import LoadingButton from "@mui/lab/LoadingButton";
import { Box, Button, MenuItem, TextField } from "@mui/material";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import APIClient from "../../util/API/APIClient";
import {
  ApprovalStatus,
  Case,
  getAllApprovalTypes,
  SimpleUser,
  CaseState,
  stringifyApproval,
} from "../../util/API/APITypes";
import { isStaff } from "../../util/UserTypeUtil";
import AddIcon from "@mui/icons-material/Add";
import { useUserContext } from "../UserContext";

interface Props {
  _case: Case;
  refreshHandler: () => void;
}

const TriageRequestCard = ({ _case }: Props) => {
  const [doctorUserSimple, setDoctorUserSimple] = useState<SimpleUser>();

  const { user } = useUserContext();

  const [approval, setApproval] = useState(ApprovalStatus.Waiting);
  const [doctorUserId, setDoctorUserId] = useState("");
  const [isApprovalPending, setIsApprovalPending] = useState(false);
  const [creating, setCreating] = useState(false);

  const handleApproval = async () => {
    setIsApprovalPending(true);
    await APIClient.cases.triageRequests.setApproval(_case.id, approval);
    setIsApprovalPending(false);
  };
  
  const handleCreate = async () => {
    setIsApprovalPending(true);
    await APIClient.cases.triageRequests.create(_case.id, doctorUserId);
    setIsApprovalPending(false);
  };

  useEffect(() => {
    if (_case.triageRequests.length > 0) {
      APIClient.profiles.getSimple(_case.triageRequests[0].doctorUserId).then((responseUser) => {
        setDoctorUserSimple(responseUser);
      });
    }
  }, []);

  const validate = () => doctorUserId.length > 0;
  
  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row" sx={{ mb: 3 }}>
          <Typography variant="h5" gutterBottom>
            Triage Requests
          </Typography>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button
              disabled={
                _case.triageRequests.length != 0 || _case.triageRequests[0].approvalStatus !== ApprovalStatus.Rejected
              }
              onClick={() => setCreating(true)}
              variant="text"
              startIcon={<AddIcon />}
            >
              Add Request
            </Button>
          </Stack>
        </Stack>

        {!creating ? null : (
          <Box>
            <TextField
              id="case-druserid-input"
              label="Doctor User ID"
              variant="outlined"
              margin="dense"
              value={doctorUserId}
              onChange={(e) => setDoctorUserId(e.target.value)}
            />
            <LoadingButton
              loading={isApprovalPending}
              disabled={!validate()}
              onClick={handleCreate}
              variant="text"
              loadingPosition="center"
            >
              Create
            </LoadingButton>
          </Box>
        )}

        {_case.state !== CaseState.WaitingTriage ? (
          <>
            <Typography variant="body2" gutterBottom>
              Approval: {stringifyApproval(_case.triageRequests[0].approvalStatus)}
            </Typography>
            <Typography variant="body2">
              Doctor Requested:{" "}
              <Link
                to={`/profiles/${doctorUserSimple?.id}`}
              >{`${doctorUserSimple?.firstName} ${doctorUserSimple?.lastName}`}</Link>
            </Typography>
            {!isStaff(user) ? null : (
              <Box>
                <TextField
                  select
                  margin="dense"
                  fullWidth
                  label="Approval"
                  onChange={(e) => setApproval(parseInt(e.target.value))}
                  value={approval}
                >
                  {getAllApprovalTypes().map((type) => (
                    <MenuItem key={type} value={type}>
                      {stringifyApproval(type)}
                    </MenuItem>
                  ))}
                </TextField>
                <Stack justifyContent="right">
                  <LoadingButton loading={isApprovalPending} onClick={handleApproval} variant="text">
                    Set Approval
                  </LoadingButton>
                </Stack>
              </Box>
            )}
          </>
        ) : (
          <Typography variant="body2">No active triage requests exist.</Typography>
        )}
      </CardContent>
    </Card>
  );
};

export default TriageRequestCard;

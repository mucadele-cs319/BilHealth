import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { ApprovalStatus, Case, CaseState } from "../../util/API/APITypes";
import AddIcon from "@mui/icons-material/Add";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import TriageRequestItem from "./TriageRequestItem";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Cancel";
import Divider from "@mui/material/Divider";

interface Props {
  _case: Case;
  refreshHandler: () => void;
}

const TriageRequestCard = ({ _case, refreshHandler }: Props) => {
  const [creating, setCreating] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [doctorUserId, setDoctorUserId] = useState("");

  const handleCreate = async () => {
    setIsPending(true);
    await APIClient.cases.triageRequests.create(_case.id, doctorUserId);
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    setDoctorUserId("");
    setCreating(false);
  };

  const validate = () => doctorUserId.length > 0;

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Triage Requests
          </Typography>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button disabled={_case.state !== CaseState.WaitingTriage} onClick={() => setCreating(true)} variant="text">
              Add Request
            </Button>
          </Stack>
        </Stack>

        {creating ? (
          <Stack justifyContent="center" direction="row" spacing={2}>
            <TextField
              id="triagerequest-id-input"
              label="Doctor User ID"
              variant="outlined"
              margin="dense"
              value={doctorUserId}
              onChange={(e) => setDoctorUserId(e.target.value)}
            />
            <Stack direction="column" justifyContent="center">
              <Button onClick={handleCancel} startIcon={<CancelIcon />}>
                Cancel
              </Button>
              <LoadingButton
                disabled={!validate()}
                loading={isPending}
                variant="text"
                loadingPosition="start"
                startIcon={creating ? <AddIcon /> : <SaveIcon />}
                onClick={handleCreate}
              >
                Request
              </LoadingButton>
            </Stack>
          </Stack>
        ) : null}

        {_case.triageRequests.length === 0 ? (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No triage requests have been made.</Typography>
          </Stack>
        ) : _case.triageRequests[0].approvalStatus !== ApprovalStatus.Waiting ? (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No active triage requests at this time.</Typography>
          </Stack>
        ) : (
          <Box>
            <TriageRequestItem triageRequest={_case.triageRequests[0]} refreshHandler={refreshHandler} />
          </Box>
        )}

        {_case.triageRequests.length > 0 &&
        _case.triageRequests.some((t) => t.approvalStatus === ApprovalStatus.Rejected) ? (
          <Box mt={2}>
            <Divider />
            <Typography mt={2} variant="h6">
              Previous Requests
            </Typography>
            <Box className="max-h-96 overflow-auto">
              {_case.triageRequests
                .filter((request) => request.approvalStatus !== ApprovalStatus.Waiting)
                .map((request) => (
                  <TriageRequestItem key={request.id} triageRequest={request} refreshHandler={refreshHandler} />
                ))}
            </Box>
          </Box>
        ) : null}
      </CardContent>
    </Card>
  );
};

export default TriageRequestCard;

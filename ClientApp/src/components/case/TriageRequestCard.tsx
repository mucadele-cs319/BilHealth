import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { ApprovalStatus, Case, CaseState, SimpleUser, UserType } from "../../util/API/APITypes";
import AddIcon from "@mui/icons-material/Add";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import TriageRequestItem from "./TriageRequestItem";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Cancel";
import Divider from "@mui/material/Divider";
import UserAutoComplete from "../UserAutoComplete";

interface Props {
  _case: Case;
  readonly: boolean;
  refreshHandler: () => void;
}

const TriageRequestCard = ({ _case, readonly, refreshHandler }: Props) => {
  const [creating, setCreating] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [doctorUser, setDoctorUser] = useState<SimpleUser | null>(null);

  const handleCreate = async () => {
    setIsPending(true);
    if (doctorUser === null) throw Error("No doctor picked");
    await APIClient.cases.triageRequests.create(_case.id, doctorUser.id);
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    setDoctorUser(null);
    setCreating(false);
  };

  const validate = () => doctorUser !== null;

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Triage Requests
          </Typography>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button
              disabled={_case.state !== CaseState.WaitingTriage || readonly || creating}
              onClick={() => setCreating(true)}
              variant="text"
            >
              Add Request
            </Button>
          </Stack>
        </Stack>

        {creating ? (
          <Stack justifyContent="center" direction="row" spacing={2}>
            <Box sx={{ minWidth: "240px", margin: 1 }}>
              <UserAutoComplete
                userType={UserType.Doctor}
                label="Doctor User"
                value={doctorUser}
                onChange={(e, v) => setDoctorUser(v)}
              />
            </Box>
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
            <TriageRequestItem
              triageRequest={_case.triageRequests[0]}
              refreshHandler={refreshHandler}
              readonly={readonly}
            />
          </Box>
        )}

        {_case.triageRequests.length > 0 ? (
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

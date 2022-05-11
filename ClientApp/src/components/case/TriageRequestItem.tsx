import LoadingButton from "@mui/lab/LoadingButton";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { ApprovalStatus, stringifyApproval, TriageRequest } from "../../util/API/APITypes";
import { isStaff } from "../../util/UserTypeUtil";
import { useUserContext } from "../UserContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";

interface Props {
  triageRequest: TriageRequest;
  readonly?: boolean;
  refreshHandler: () => void;
}

const TriageRequestItem = ({ triageRequest, readonly = false, refreshHandler }: Props) => {
  const active = triageRequest.approvalStatus === ApprovalStatus.Waiting;

  const { user } = useUserContext();

  const [isPending, setIsPending] = useState(false);
  const [attemptingApproval, setAttemptingApproval] = useState(false);
  const [approval, setApproval] = useState(ApprovalStatus.Approved);

  const handleApprove = async () => {
    setIsPending(true);
    await APIClient.cases.triageRequests.setApproval(triageRequest.caseId, approval);
    setIsPending(false);
    setAttemptingApproval(false);
    refreshHandler();
  };

  const handleAttempt = (approval: ApprovalStatus) => {
    setApproval(approval);
    setAttemptingApproval(true);
  };

  return (
    <Box pl={1} mb={active ? 0 : 2} sx={{ borderColor: "darkred" }} borderLeft={active ? 0 : 1}>
      {active ? (
        <Typography variant="h6" color="green">
          Currently Active
        </Typography>
      ) : null}
      <Typography variant="caption" mb={1} component="p">
        Requested on {triageRequest.dateTime.format("DD/MM/YYYY, HH:mm")} by {triageRequest.requestingUserId}
      </Typography>
      <Typography variant="body2">Status: {stringifyApproval(triageRequest.approvalStatus)}</Typography>
      <Typography variant="body2" gutterBottom>
        Requested Doctor: {triageRequest.doctorUserId}
      </Typography>
      {active ? (
        <Stack direction="row" spacing={1}>
          <LoadingButton
            disabled={readonly}
            loading={isPending}
            onClick={() => handleAttempt(ApprovalStatus.Rejected)}
            variant="text"
            loadingPosition="center"
          >
            Reject
          </LoadingButton>
          <LoadingButton
            disabled={!isStaff(user)}
            loading={isPending}
            onClick={() => handleAttempt(ApprovalStatus.Approved)}
            variant="text"
            loadingPosition="center"
          >
            Approve
          </LoadingButton>
          <Dialog open={attemptingApproval} onClose={() => setAttemptingApproval(false)}>
            <DialogTitle>Confirm Triage Request Status Change</DialogTitle>
            <DialogContent>
              <DialogContentText>
                Are you sure you want to {approval === ApprovalStatus.Approved ? "approve" : "reject"} this triage
                request?
              </DialogContentText>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setAttemptingApproval(false)}>No</Button>
              <Button onClick={handleApprove} autoFocus>
                Yes
              </Button>
            </DialogActions>
          </Dialog>
        </Stack>
      ) : null}
    </Box>
  );
};

export default TriageRequestItem;

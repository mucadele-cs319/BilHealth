import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Case, CaseState, stringifyCaseState, stringifyCaseType } from "../../util/API/APITypes";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";

interface Props {
  _case: Case;
  refreshHandler: () => void;
}

const CaseHeaderCard = ({ _case, refreshHandler }: Props) => {
  const [isUnassignPending, setIsUnassignPending] = useState(false);
  const [unassignAttempt, setUnassignAttempt] = useState(false);

  const [isStatePending, setIsStatePending] = useState(false);
  const [togglingState, setTogglingState] = useState(false);

  const [nextState, setNextState] = useState(CaseState.Closed);

  const determineNextState = () => {
    if (_case.state !== CaseState.Closed) setNextState(CaseState.Closed);
    else if (_case.doctorUserId === null) setNextState(CaseState.WaitingTriage);
    else setNextState(CaseState.Ongoing);
  };

  useEffect(() => {
    determineNextState();
  }, [_case]);

  const handleStateChange = async () => {
    setIsStatePending(true);
    await APIClient.cases.changeState(_case.id, nextState);
    setIsStatePending(false);
    setTogglingState(false);
    refreshHandler();
  };

  const handleUnassign = async () => {
    setIsUnassignPending(true);
    await APIClient.cases.unassign(_case.id);
    setIsUnassignPending(false);
    setUnassignAttempt(false);
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Case: {_case.title}
        </Typography>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          Opened on {_case.dateTime.format("DD/MM/YYYY, HH:mm")} by{" "}
          {`${_case.simplePatientUser?.firstName} ${_case.simplePatientUser?.lastName}`}
        </Typography>
        <Typography variant="body2" gutterBottom>
          Medically related to <strong>{stringifyCaseType(_case.type)}</strong>.
        </Typography>
        <Typography variant="body2" gutterBottom>
          Currently <strong>{stringifyCaseState(_case.state)}</strong>.
        </Typography>

        <Stack direction="row" justifyContent="end" spacing={1}>
          <LoadingButton
            disabled={_case.doctorUserId === null}
            loading={isUnassignPending}
            onClick={() => setUnassignAttempt(true)}
            variant="text"
            color="error"
            loadingPosition="center"
          >
            Unassign Doctor
          </LoadingButton>
          <LoadingButton
            loading={isStatePending}
            onClick={() => setTogglingState(true)}
            variant="text"
            color="error"
            loadingPosition="center"
          >
            {nextState === CaseState.Closed ? "Close" : "Reopen"} Case
          </LoadingButton>
        </Stack>

        <Dialog open={unassignAttempt} onClose={() => setUnassignAttempt(false)}>
          <DialogTitle>Confirm Doctor Unassignment</DialogTitle>
          <DialogContent>
            <DialogContentText>Are you sure you want to unassign this case?</DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setUnassignAttempt(false)}>No</Button>
            <Button onClick={handleUnassign} autoFocus>
              Yes
            </Button>
          </DialogActions>
        </Dialog>

        <Dialog open={togglingState} onClose={() => setTogglingState(false)}>
          <DialogTitle>Confirm Case State Change</DialogTitle>
          <DialogContent>
            <DialogContentText>
              Are you sure you want to {nextState === CaseState.Closed ? "close" : "reopen"} this case?
            </DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setTogglingState(false)}>No</Button>
            <Button onClick={handleStateChange} autoFocus>
              Yes
            </Button>
          </DialogActions>
        </Dialog>
      </CardContent>
    </Card>
  );
};

export default CaseHeaderCard;

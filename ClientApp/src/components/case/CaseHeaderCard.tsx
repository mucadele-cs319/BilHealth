import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Case, CaseState, stringifyCaseState, stringifyCaseType, UserType } from "../../util/API/APITypes";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import { useUserContext } from "../UserContext";
import { fullNameify } from "../../util/StringUtil";

interface Props {
  _case: Case;
  readonly: boolean;
  refreshHandler: () => void;
}

const CaseHeaderCard = ({ _case, readonly, refreshHandler }: Props) => {
  const { user } = useUserContext();

  const [isDiagnosisPending, setIsDiagnosisPending] = useState(false);
  const [makingDiagnosis, setMakingDiagnosis] = useState(false);
  const [diagnosis, setDiagnosis] = useState(_case.diagnosis);

  const [isUnassignPending, setIsUnassignPending] = useState(false);
  const [unassignAttempt, setUnassignAttempt] = useState(false);

  const [isStatePending, setIsStatePending] = useState(false);
  const [togglingState, setTogglingState] = useState(false);

  const [nextState, setNextState] = useState(CaseState.Closed);

  const determineNextState = () => {
    if (_case.state !== CaseState.Closed) setNextState(CaseState.Closed);
    else if (_case.doctorUser === null) setNextState(CaseState.WaitingTriage);
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
    setUnassignAttempt(false);
    setMakingDiagnosis(false);
    refreshHandler();
  };

  const handleUnassign = async () => {
    setIsUnassignPending(true);
    await APIClient.cases.unassign(_case.id);
    setIsUnassignPending(false);
    setUnassignAttempt(false);
    refreshHandler();
  };

  const handleDiagnosis = async () => {
    setIsDiagnosisPending(true);
    await APIClient.cases.diagnosis(_case.id, { content: diagnosis });
    setIsDiagnosisPending(false);
    setMakingDiagnosis(false);
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Case: {_case.title}
        </Typography>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          Opened on {_case.dateTime.format("DD/MM/YYYY, HH:mm")} by {fullNameify(_case.patientUser)}
        </Typography>
        <Typography variant="body2" gutterBottom>
          Medically related to <strong>{stringifyCaseType(_case.type)}</strong>.
        </Typography>
        <Typography variant="body2" gutterBottom>
          The case is <strong>{stringifyCaseState(_case.state)}</strong>.
        </Typography>
        <Typography variant="body2" gutterBottom>
          {_case.diagnosis === null || _case.diagnosis.length === 0 ? (
            <>
              Currently <strong>undiagnosed</strong>.
            </>
          ) : (
            <>
              Diagnosed with: <strong>{_case.diagnosis}</strong>.
            </>
          )}
        </Typography>

        <Stack direction="row" justifyContent="end" spacing={1}>
          {user?.userType === UserType.Doctor && !readonly ? (
            <Button
              disabled={makingDiagnosis || _case.state === CaseState.Closed}
              onClick={() => setMakingDiagnosis(true)}
              variant="text"
            >
              Make Diagnosis
            </Button>
          ) : null}
          <LoadingButton
            disabled={_case.doctorUser === null || _case.state === CaseState.Closed || readonly}
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
            disabled={readonly}
            onClick={() => setTogglingState(true)}
            variant="text"
            color="error"
            loadingPosition="center"
          >
            {nextState === CaseState.Closed ? "Close" : "Reopen"} Case
          </LoadingButton>
        </Stack>

        {!makingDiagnosis ? null : (
          <Box>
            <Stack direction="row" justifyContent="end" spacing={1}>
              <TextField
                id="case-diag-input"
                label="Diagnosis"
                variant="outlined"
                margin="dense"
                value={diagnosis}
                onChange={(e) => setDiagnosis(e.target.value)}
              />
              <Button
                onClick={() => {
                  setMakingDiagnosis(false);
                  setDiagnosis(_case.diagnosis);
                }}
                variant="text"
              >
                Cancel
              </Button>
              <LoadingButton
                loading={isDiagnosisPending}
                onClick={handleDiagnosis}
                variant="text"
                loadingPosition="center"
              >
                Save
              </LoadingButton>
            </Stack>
          </Box>
        )}

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

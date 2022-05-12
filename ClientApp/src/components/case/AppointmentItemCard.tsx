import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Appointment, Case, CaseState, stringifyCaseState, stringifyCaseType } from "../../util/API/APITypes";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import { fmtConcise } from "../../util/StringUtil";

interface Props {
  _case: Case;
  appointment: Appointment;
  refreshHandler: () => void;
}

const AppointmentItemCard = ({ _case, appointment, refreshHandler }: Props) => {
  const [isPending, setIsPending] = useState(false);
  const [closing, setClosing] = useState(false);

  const handleClose = async () => {
    setIsPending(true);
    await APIClient.cases.changeState(_case.id, CaseState.Closed);
    setIsPending(false);
    setClosing(false);
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          On {appointment.dateTime.format(fmtConcise)}
        </Typography>
        <Typography variant="body2" color="text.secondary" gutterBottom>
          Created on {appointment.createdAt.format(fmtConcise)}
        </Typography>
        <Typography variant="body2" gutterBottom>
          Medically related to <strong>{stringifyCaseType(_case.type)}</strong>.
        </Typography>
        <Typography variant="body2" gutterBottom>
          Currently <strong>{stringifyCaseState(_case.state)}</strong>.
        </Typography>

        <Stack direction="row" justifyContent="end">
          <LoadingButton
            loading={isPending}
            disabled={_case.state === CaseState.Closed}
            onClick={() => setClosing(true)}
            variant="text"
            color="error"
            loadingPosition="center"
          >
            Close Case
          </LoadingButton>
        </Stack>

        <Dialog open={closing} onClose={() => setClosing(false)}>
          <DialogTitle>Confirm Case Closure</DialogTitle>
          <DialogContent>
            <DialogContentText>Are you sure you want to close this case?</DialogContentText>
          </DialogContent>
          <DialogActions>
            <Button onClick={() => setClosing(false)}>No</Button>
            <Button onClick={handleClose} autoFocus>
              Yes
            </Button>
          </DialogActions>
        </Dialog>
      </CardContent>
    </Card>
  );
};

export default AppointmentItemCard;

import LoadingButton from "@mui/lab/LoadingButton";
import Box from "@mui/material/Box";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Appointment, ApprovalStatus, stringifyApproval, UserType } from "../../util/API/APITypes";
import { isStaff } from "../../util/UserTypeUtil";
import { useUserContext } from "../UserContext";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import { fmtConcise, fmtReadableDetailed, linkUser } from "../../util/StringUtil";
import dayjs from "dayjs";
import Grid from "@mui/material/Grid";
import InputAdornment from "@mui/material/InputAdornment";
import TextField from "@mui/material/TextField";

interface Props {
  appointment: Appointment;
  readonly?: boolean;
  refreshHandler: () => void;
}

const AppointmentItemCard = ({ appointment, readonly = false, refreshHandler }: Props) => {
  const past = dayjs().subtract(3, "hours").isAfter(appointment.dateTime);
  const waiting = !past && appointment.approvalStatus === ApprovalStatus.Waiting;
  const upcoming = !past && appointment.approvalStatus === ApprovalStatus.Approved && !appointment.cancelled;

  const { user } = useUserContext();

  const [isPending, setIsPending] = useState(false);
  const [attemptingApproval, setAttemptingApproval] = useState(false);
  const [approval, setApproval] = useState(ApprovalStatus.Approved);

  const [attemptingCancel, setAttemptingCancel] = useState(false);

  const handleApprove = async () => {
    setIsPending(true);
    await APIClient.appointments.setApproval(appointment.id, approval);
    setIsPending(false);
    setAttemptingApproval(false);
    refreshHandler();
  };

  const handleAttempt = (approval: ApprovalStatus) => {
    setApproval(approval);
    setAttemptingApproval(true);
  };

  const handleCancel = async () => {
    setIsPending(true);
    await APIClient.appointments.cancel(appointment.id);
    setIsPending(false);
    setAttemptingCancel(false);
    refreshHandler();
  };

  const [attemptingVisit, setAttemptingVisit] = useState(false);
  const [updatingVisit, setUpdatingVisit] = useState(false);
  const [bpm, setBpm] = useState<number | undefined>(appointment.visit?.bpm);
  const [bloodPressure, setBloodPressure] = useState<number | undefined>(appointment.visit?.bloodPressure);
  const [bodyTemperature, setBodyTemperature] = useState<number | undefined>(appointment.visit?.bodyTemperature);
  const [notes, setNotes] = useState<string | undefined>(appointment.visit?.notes);

  const handleCreateVisit = async () => {
    setIsPending(true);
    await APIClient.appointments.visits.create(appointment.id);
    setIsPending(false);
    setAttemptingVisit(false);
    refreshHandler();
  };

  const handleVisitUpdate = async () => {
    setIsPending(true);
    await APIClient.appointments.visits.update(appointment.id, { bpm, bloodPressure, bodyTemperature, notes });
    setIsPending(false);
    setUpdatingVisit(false);
    refreshHandler();
  };

  return (
    <Box
      pl={1}
      mb={waiting || upcoming ? 0 : 2}
      sx={{ borderColor: "darkred" }}
      borderLeft={waiting || upcoming ? 0 : 1}
    >
      {waiting ? (
        <Typography variant="h6" color="darkorange">
          Pending Appointment
        </Typography>
      ) : upcoming ? (
        <Typography variant="h6" color="green">
          Upcoming Appointment
        </Typography>
      ) : null}
      <Typography variant="h6" fontSize="medium">
        Scheduled for {appointment.dateTime.format(fmtReadableDetailed)}
      </Typography>
      <Typography variant="caption" mb={1} component="p">
        Created on {appointment.createdAt.format(fmtConcise)} by {linkUser(appointment.requestingUser)}
      </Typography>
      <Typography variant="body2">
        Status: {stringifyApproval(appointment.approvalStatus)}
        {appointment.cancelled ? " then canceled" : null}
      </Typography>
      {appointment.description ? <Typography variant="body2">Description: {appointment.description}</Typography> : null}
      {appointment.approvalStatus === ApprovalStatus.Approved && !appointment.cancelled ? (
        <Typography variant="body2">Attended: {appointment.attended ? "Yes" : "No"}</Typography>
      ) : null}

      {waiting ? (
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
            <DialogTitle>Confirm Appointment Status Change</DialogTitle>
            <DialogContent>
              <DialogContentText>
                Are you sure you want to {approval === ApprovalStatus.Approved ? "approve" : "reject"} this appointment?
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
      ) : upcoming ? (
        <Stack direction="row" spacing={1}>
          <LoadingButton
            disabled={readonly || Boolean(appointment.visit)}
            loading={isPending}
            onClick={() => setAttemptingCancel(true)}
            variant="text"
            loadingPosition="center"
            color="error"
          >
            Cancel
          </LoadingButton>
          {appointment.visit ? (
            <Button
              disabled={
                readonly ||
                updatingVisit ||
                [UserType.Admin, UserType.Doctor, UserType.Nurse].some((type) => type === user?.userType) === false
              }
              onClick={() => setUpdatingVisit(true)}
            >
              View Visit
            </Button>
          ) : (
            <LoadingButton
              disabled={
                readonly ||
                [UserType.Admin, UserType.Doctor, UserType.Nurse].some((type) => type === user?.userType) === false
              }
              loading={isPending}
              onClick={() => setAttemptingVisit(true)}
              variant="text"
              loadingPosition="center"
            >
              Create Visit
            </LoadingButton>
          )}

          <Dialog open={attemptingCancel} onClose={() => setAttemptingCancel(false)}>
            <DialogTitle>Confirm Appointment Cancellation</DialogTitle>
            <DialogContent>
              <DialogContentText>Are you sure you want to cancel this appointment?</DialogContentText>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setAttemptingCancel(false)}>No</Button>
              <Button onClick={handleCancel} autoFocus>
                Yes
              </Button>
            </DialogActions>
          </Dialog>

          <Dialog open={attemptingVisit} onClose={() => setAttemptingVisit(false)}>
            <DialogTitle>Confirm Appointment Visit Creation</DialogTitle>
            <DialogContent>
              <DialogContentText>Are you sure you want to create a visit for this appointment?</DialogContentText>
            </DialogContent>
            <DialogActions>
              <Button onClick={() => setAttemptingVisit(false)}>No</Button>
              <Button onClick={handleCreateVisit} autoFocus>
                Yes
              </Button>
            </DialogActions>
          </Dialog>
        </Stack>
      ) : null}

      {updatingVisit ? (
        <Box mt={1}>
          <Typography variant="subtitle1">Visit Details</Typography>
          <Grid container spacing={2} justifyContent="center">
            <Grid item>
              <TextField
                id="case-appointmentvisit-bpm"
                label="BPM"
                variant="outlined"
                margin="dense"
                value={bpm}
                type="number"
                inputProps={{
                  step: "0.5",
                }}
                onChange={(e) => setBpm(parseFloat(e.target.value))}
              />
            </Grid>
            <Grid item>
              <TextField
                id="case-appointmentvisit-bt"
                label="Body Temperature"
                variant="outlined"
                margin="dense"
                value={bodyTemperature}
                type="number"
                inputProps={{
                  step: "0.5",
                }}
                InputProps={{
                  endAdornment: <InputAdornment position="end">°C</InputAdornment>,
                }}
                onChange={(e) => setBodyTemperature(parseFloat(e.target.value))}
              />
            </Grid>
            <Grid item>
              <TextField
                id="case-appointmentvisit-bp"
                label="Blood Pressure"
                variant="outlined"
                margin="dense"
                value={bloodPressure}
                type="number"
                inputProps={{
                  step: "0.5",
                }}
                InputProps={{
                  endAdornment: <InputAdornment position="end">mmHg</InputAdornment>,
                }}
                onChange={(e) => setBloodPressure(parseFloat(e.target.value))}
              />
            </Grid>
            <Grid item xs={12}>
              <TextField
                id="case-appointmentvisit-notes"
                label="Notes"
                variant="filled"
                multiline
                fullWidth
                margin="dense"
                minRows={4}
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
              />
            </Grid>
          </Grid>

          {[UserType.Admin, UserType.Nurse, UserType.Doctor].some((type) => type == user?.userType) ? (
            <Stack direction="row" justifyContent="end">
              <Button variant="text" onClick={() => setUpdatingVisit(false)}>
                Cancel
              </Button>
              <LoadingButton loading={isPending} onClick={handleVisitUpdate} variant="text" loadingPosition="center">
                Save
              </LoadingButton>
            </Stack>
          ) : null}
        </Box>
      ) : null}
    </Box>
  );
};

export default AppointmentItemCard;

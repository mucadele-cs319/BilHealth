import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { ApprovalStatus, Case } from "../../util/API/APITypes";
import AddIcon from "@mui/icons-material/Add";
import TextField from "@mui/material/TextField";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import SaveIcon from "@mui/icons-material/Save";
import CancelIcon from "@mui/icons-material/Cancel";
import Divider from "@mui/material/Divider";
import { useUserContext } from "../UserContext";
import { isPatient } from "../../util/UserTypeUtil";
import DateTimePicker from "@mui/lab/DateTimePicker";
import LocalizationProvider from "@mui/lab/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import dayjs, { Dayjs } from "dayjs";
import AppointmentItemCard from "./AppointmentItemCard";

interface Props {
  _case: Case;
  readonly: boolean;
  refreshHandler: () => void;
}

const AppointmentCard = ({ _case, readonly, refreshHandler }: Props) => {
  const { user } = useUserContext();

  const [creating, setCreating] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [description, setDescription] = useState("");
  const [date, setDate] = useState<Dayjs | null>(null);

  const handleCreate = async () => {
    setIsPending(true);
    if (date === null) throw Error("No date picked");
    await APIClient.appointments.create(_case.id, { dateTime: date, description });
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    setDescription("");
    setDate(null);
    setCreating(false);
  };

  const validate = () => date !== null;

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Appointments
          </Typography>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button
              disabled={
                (_case.appointments.length > 0 &&
                  (_case.appointments[0].approvalStatus === ApprovalStatus.Waiting ||
                    (_case.appointments[0].approvalStatus === ApprovalStatus.Approved &&
                      _case.appointments[0].cancelled === false &&
                      _case.appointments[0].dateTime.isAfter(dayjs().subtract(3, "hours"))))) ||
                user?.blacklisted ||
                readonly ||
                creating
              }
              onClick={() => setCreating(true)}
              variant="text"
            >
              {isPatient(user) ? "Add Request" : "Create"}
            </Button>
          </Stack>
        </Stack>

        {creating ? (
          <Stack justifyContent="center" direction="row" spacing={2}>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DateTimePicker
                minDateTime={dayjs()}
                minutesStep={5}
                maxDate={dayjs().add(4, "weeks")}
                ampm={false}
                label="Date"
                value={date}
                onChange={(date) => setDate(date || dayjs())}
                renderInput={(params) => <TextField {...params} />}
              />
            </LocalizationProvider>
            <TextField
              id="appointment-desc-input"
              label="Description"
              variant="outlined"
              margin="dense"
              value={description}
              onChange={(e) => setDescription(e.target.value)}
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
                {isPatient(user) ? "Request" : "Create"}
              </LoadingButton>
            </Stack>
          </Stack>
        ) : null}

        {_case.appointments.length === 0 ? (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No appointments have been made.</Typography>
          </Stack>
        ) : _case.appointments[0].approvalStatus === ApprovalStatus.Rejected ||
          (_case.appointments[0].approvalStatus === ApprovalStatus.Approved &&
            (_case.appointments[0].cancelled ||
              _case.appointments[0].dateTime.isBefore(dayjs().subtract(3, "hours")))) ? (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No active appointments at this time.</Typography>
          </Stack>
        ) : (
          <Box>
            <AppointmentItemCard
              appointment={_case.appointments[0]}
              refreshHandler={refreshHandler}
              readonly={readonly}
            />
          </Box>
        )}

        {_case.appointments.length > 0 &&
        _case.appointments.some((t) => t.approvalStatus === ApprovalStatus.Rejected) ? (
          <Box mt={2}>
            <Divider />
            <Typography mt={2} variant="h6">
              Past Appointments
            </Typography>
            <Box className="max-h-96 overflow-auto">
              {_case.appointments
                .filter(
                  (appointment) =>
                    appointment.approvalStatus === ApprovalStatus.Rejected ||
                    (appointment.approvalStatus === ApprovalStatus.Approved &&
                      (appointment.cancelled || appointment.dateTime.isBefore(dayjs().subtract(3, "hours")))),
                )
                .map((appointment) => (
                  <AppointmentItemCard key={appointment.id} appointment={appointment} refreshHandler={refreshHandler} />
                ))}
            </Box>
          </Box>
        ) : null}
      </CardContent>
    </Card>
  );
};

export default AppointmentCard;

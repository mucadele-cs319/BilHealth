import IconButton from "@mui/material/IconButton";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import React, { useState } from "react";
import { Vaccination } from "../../util/API/APITypes";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import SaveIcon from "@mui/icons-material/Save";
import AddIcon from "@mui/icons-material/Add";
import CancelIcon from "@mui/icons-material/Cancel";
import APIClient from "../../util/API/APIClient";
import TextField from "@mui/material/TextField";
import dayjs from "dayjs";
import LoadingButton from "@mui/lab/LoadingButton";
import DateTimePicker from "@mui/lab/DateTimePicker";
import LocalizationProvider from "@mui/lab/LocalizationProvider";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import Tooltip from "@mui/material/Tooltip";

interface Props {
  readonly?: boolean;
  patientId: string;
  vaccination?: Vaccination;
  refreshHandler: () => void;
  cancelHandler?: () => void;
}

const VaccinationRow = ({ readonly = false, patientId, vaccination, refreshHandler, cancelHandler }: Props) => {
  const creating = vaccination === undefined;

  const [editing, setEditing] = useState(creating);
  const [deleting, setDeleting] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [newType, setNewType] = useState(vaccination?.type || "");
  const [newDate, setNewDate] = useState(vaccination?.dateTime || dayjs());

  const handleEditSave = async () => {
    setIsPending(true);

    if (vaccination === undefined) throw Error("No existing vaccination");
    vaccination.type = newType;
    vaccination.dateTime = newDate;
    await APIClient.profiles.vaccination.update(vaccination.id, vaccination);

    setIsPending(false);
    setEditing(false);
    refreshHandler();
  };

  const handleDelete = async () => {
    setIsPending(true);

    if (vaccination?.id) await APIClient.profiles.vaccination.delete(vaccination.id);

    setIsPending(false);
    refreshHandler();
  };

  const handleCreate = async () => {
    setIsPending(true);

    await APIClient.profiles.vaccination.add(patientId, { type: newType, dateTime: newDate });
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    if (creating && cancelHandler) cancelHandler();
    else setEditing(false);
  };

  const validate = () => newType.length > 0;

  return (
    <TableRow hover>
      {editing ? (
        <>
          <TableCell>
            <TextField
              id="vaccination-type-input"
              label="Type"
              variant="outlined"
              margin="dense"
              value={newType}
              onChange={(e) => setNewType(e.target.value)}
            />
          </TableCell>
          <TableCell>
            <LocalizationProvider dateAdapter={AdapterDayjs}>
              <DateTimePicker
                ampm={false}
                label="Date"
                value={newDate}
                onChange={(date) => setNewDate(date || dayjs())}
                renderInput={(params) => <TextField {...params} />}
              />
            </LocalizationProvider>
          </TableCell>
          <TableCell align="right">
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
                onClick={creating ? handleCreate : handleEditSave}
              >
                {creating ? "Add" : "Save"}
              </LoadingButton>
            </Stack>
          </TableCell>
        </>
      ) : (
        <>
          <TableCell>{vaccination?.type}</TableCell>
          <TableCell>{vaccination?.dateTime?.format("DD/MM/YYYY, HH:mm")}</TableCell>
          {readonly ? null : (
            <>
              <TableCell align="right">
                <Tooltip arrow title="Edit">
                  <IconButton onClick={() => setEditing(true)}>
                    <EditIcon />
                  </IconButton>
                </Tooltip>
                <Tooltip arrow title="Delete">
                  <IconButton onClick={() => setDeleting(true)}>
                    <DeleteIcon />
                  </IconButton>
                </Tooltip>
              </TableCell>
              <Dialog open={deleting} onClose={() => setDeleting(false)}>
                <DialogTitle>Confirm Deletion</DialogTitle>
                <DialogContent>
                  <DialogContentText>
                    Are you sure you want to delete the vaccination <strong>{vaccination?.type}</strong>?
                  </DialogContentText>
                </DialogContent>
                <DialogActions>
                  <Button onClick={() => setDeleting(false)}>No</Button>
                  <Button onClick={handleDelete} autoFocus>
                    Yes
                  </Button>
                </DialogActions>
              </Dialog>
            </>
          )}
        </>
      )}
    </TableRow>
  );
};

export default VaccinationRow;

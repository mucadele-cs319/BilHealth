import IconButton from "@mui/material/IconButton";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import React, { useState } from "react";
import { Case, Prescription } from "../../util/API/APITypes";
import AddIcon from "@mui/icons-material/Add";
import CancelIcon from "@mui/icons-material/Cancel";
import APIClient from "../../util/API/APIClient";
import TextField from "@mui/material/TextField";
import dayjs from "dayjs";
import LoadingButton from "@mui/lab/LoadingButton";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import Tooltip from "@mui/material/Tooltip";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { fmtConcise } from "../../util/StringUtil";

interface Props {
  _case: Case;
  readonly?: boolean;
  prescription?: Prescription;
  refreshHandler: () => void;
  cancelHandler?: () => void;
}

const PrescriptionCardRow = ({ _case, prescription, readonly = false, refreshHandler, cancelHandler }: Props) => {
  const creating = prescription === undefined;

  const [editing, setEditing] = useState(creating);
  const [deleting, setDeleting] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [newItem, setNewItem] = useState(prescription?.item || "");

  const handleDelete = async () => {
    setIsPending(true);

    if (prescription?.id) await APIClient.cases.prescriptions.delete(prescription.id);

    setIsPending(false);
    setDeleting(false);
    refreshHandler();
  };

  const handleEditSave = async () => {
    setIsPending(true);

    if (prescription === undefined) throw Error("No existing prescription");
    await APIClient.cases.prescriptions.update(prescription.id, { item: newItem });

    setIsPending(false);
    setEditing(false);
    refreshHandler();
  };

  const handleCreate = async () => {
    setIsPending(true);

    await APIClient.cases.prescriptions.add(_case.id, {
      item: newItem,
    });
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    if (creating && cancelHandler) cancelHandler();
    else setEditing(false);
  };

  const validate = () => newItem.length > 0;

  return (
    <TableRow hover>
      {editing ? (
        <>
          <TableCell>{dayjs().format(fmtConcise)}</TableCell>
          <TableCell>
            <TextField
              id="presc-user-input"
              label="Prescription Item"
              variant="outlined"
              margin="dense"
              value={newItem}
              onChange={(e) => setNewItem(e.target.value)}
            />
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
                startIcon={<AddIcon />}
                onClick={creating ? handleCreate : handleEditSave}
              >
                {creating ? "Add" : "Edit"}
              </LoadingButton>
            </Stack>
          </TableCell>
        </>
      ) : (
        <>
          <TableCell>{prescription?.dateTime.format(fmtConcise)}</TableCell>
          <TableCell>{prescription?.item}</TableCell>
          <TableCell align="right">
            {readonly ? null : (
              <>
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
              </>
            )}
          </TableCell>

          <Dialog open={deleting} onClose={() => setDeleting(false)}>
            <DialogTitle>Confirm Deletion</DialogTitle>
            <DialogContent>
              <DialogContentText>Are you sure you want to delete the prescription?</DialogContentText>
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
    </TableRow>
  );
};

export default PrescriptionCardRow;

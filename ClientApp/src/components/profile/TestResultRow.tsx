import IconButton from "@mui/material/IconButton";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import React, { useState } from "react";
import { getAllMedicalTestTypes, MedicalTestType, stringifyMedicalTest, TestResult } from "../../util/API/APITypes";
import FileOpenIcon from "@mui/icons-material/FileOpen";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import SaveIcon from "@mui/icons-material/Save";
import AddIcon from "@mui/icons-material/Add";
import CancelIcon from "@mui/icons-material/Cancel";
import APIClient from "../../util/API/APIClient";
import TextField from "@mui/material/TextField";
import LoadingButton from "@mui/lab/LoadingButton";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import Stack from "@mui/material/Stack";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";
import MenuItem from "@mui/material/MenuItem";

interface Props {
  readonly?: boolean;
  patientId: string;
  testResult?: TestResult;
  refreshHandler: () => void;
  cancelHandler?: () => void;
}

const TestResultRow = ({ readonly = false, patientId, testResult, refreshHandler, cancelHandler }: Props) => {
  const creating = testResult === undefined;

  const [editing, setEditing] = useState(creating);
  const [deleting, setDeleting] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [newType, setNewType] = useState(testResult?.type || MedicalTestType.Blood);
  const [newFile, setNewFile] = useState<File>();

  const handleEditSave = async () => {
    setIsPending(true);

    if (testResult === undefined) throw Error("No existing vaccination");
    await APIClient.testResults.update(testResult.id, newType, newFile);

    setIsPending(false);
    setEditing(false);
    refreshHandler();
  };

  const handleDelete = async () => {
    setIsPending(true);

    if (testResult?.id) await APIClient.testResults.delete(testResult.id);

    setIsPending(false);
    refreshHandler();
  };

  const handleCreate = async () => {
    setIsPending(true);

    if (newType === undefined) throw Error("No type chosen");
    await APIClient.testResults.add(patientId, newType, newFile);
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    if (creating && cancelHandler) cancelHandler();
    else setEditing(false);
  };

  const validateCreation = () => newFile !== undefined;

  return (
    <TableRow hover>
      {editing ? (
        <>
          <TableCell>
            <TextField
              id="testresult-type-input"
              select
              margin="dense"
              variant="outlined"
              label="Type"
              fullWidth
              onChange={(e) => setNewType(parseInt(e.target.value))}
              value={newType}
            >
              {getAllMedicalTestTypes().map((type) => (
                <MenuItem key={type} value={type}>
                  {stringifyMedicalTest(type)}
                </MenuItem>
              ))}
            </TextField>
          </TableCell>
          <TableCell>
            <label htmlFor="testresult-button-file">
              <input
                style={{ display: "none" }}
                accept="application/pdf"
                id="testresult-button-file"
                type="file"
                onChange={(e) => setNewFile(e.target.files?.[0])}
              />
              <Button variant="contained" component="span">
                Upload
              </Button>
            </label>
            <Tooltip arrow title="Clear file">
              <Typography
                onClick={() => setNewFile(undefined)}
                sx={{ cursor: "pointer" }}
                variant="caption"
                color="text.secondary"
                m={1}
              >
                {newFile ? newFile.name : "Choose result file..."}
              </Typography>
            </Tooltip>
          </TableCell>
          <TableCell align="right">
            <Stack direction="column" justifyContent="center">
              <Button onClick={handleCancel} startIcon={<CancelIcon />}>
                Cancel
              </Button>
              <LoadingButton
                disabled={creating && !validateCreation()}
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
          <TableCell>{stringifyMedicalTest(testResult?.type as MedicalTestType)}</TableCell>
          <TableCell>{testResult?.dateTime?.format("DD/MM/YYYY, HH:mm")}</TableCell>
          <TableCell align="right">
            <Tooltip arrow title="View">
              <IconButton
                onClick={() => window.open(APIClient.testResults.getFile(testResult?.id as string), "_blank")}
              >
                <FileOpenIcon />
              </IconButton>
            </Tooltip>
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

          {readonly ? null : (
            <Dialog open={deleting} onClose={() => setDeleting(false)}>
              <DialogTitle>Confirm Deletion</DialogTitle>
              <DialogContent>
                <DialogContentText>Are you sure you want to delete this test result?</DialogContentText>
              </DialogContent>
              <DialogActions>
                <Button onClick={() => setDeleting(false)}>No</Button>
                <Button onClick={handleDelete} autoFocus>
                  Yes
                </Button>
              </DialogActions>
            </Dialog>
          )}
        </>
      )}
    </TableRow>
  );
};

export default TestResultRow;

import IconButton from "@mui/material/IconButton";
import TableCell from "@mui/material/TableCell";
import TableRow from "@mui/material/TableRow";
import React, { useState } from "react";
import { SimpleUser, TimedAccessGrant, UserType } from "../../util/API/APITypes";
import AddIcon from "@mui/icons-material/Add";
import CancelIcon from "@mui/icons-material/Cancel";
import APIClient from "../../util/API/APIClient";
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
import { fmtConcise, linkUser } from "../../util/StringUtil";
import Box from "@mui/material/Box";
import UserAutoComplete from "../UserAutoComplete";

interface Props {
  patientId: string;
  grant?: TimedAccessGrant;
  refreshHandler: () => void;
  cancelHandler?: () => void;
}

const TimedGrantRow = ({ patientId, grant, refreshHandler, cancelHandler }: Props) => {
  const creating = grant === undefined;

  const [editing, setEditing] = useState(creating);
  const [deleting, setDeleting] = useState(false);
  const [isPending, setIsPending] = useState(false);

  const [nurseUser, setNurseUser] = useState<SimpleUser | null>(null);

  const handleDelete = async () => {
    setIsPending(true);

    if (grant?.id) await APIClient.profiles.accessControl.cancelTimedAccess(patientId, grant.id);

    setIsPending(false);
    setDeleting(false);
    refreshHandler();
  };

  const handleCreate = async () => {
    setIsPending(true);

    if (nurseUser === null) throw Error("Nurse not picked");

    await APIClient.profiles.accessControl.grantTimedAccess(patientId, {
      period: "P1D",
      patientUserId: patientId,
      userId: nurseUser?.id,
    });
    setIsPending(false);
    handleCancel();
    refreshHandler();
  };

  const handleCancel = () => {
    if (creating && cancelHandler) cancelHandler();
    else setEditing(false);
  };

  const validate = () => nurseUser !== null;

  const isInactive = (grant?: TimedAccessGrant) => grant && (grant.canceled || grant.expiryTime.isBefore(dayjs()));

  return (
    <TableRow hover>
      {editing ? (
        <>
          <TableCell>{dayjs().add(dayjs.duration("P1D")).format(fmtConcise)}</TableCell>
          <TableCell>
            <Box sx={{ margin: 1 }}>
              <UserAutoComplete
                userType={UserType.Nurse}
                label="Nurse User"
                value={nurseUser}
                onChange={(e, v) => setNurseUser(v)}
              />
            </Box>
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
                onClick={handleCreate}
              >
                Grant
              </LoadingButton>
            </Stack>
          </TableCell>
        </>
      ) : (
        <>
          <Tooltip arrow title={isInactive(grant) ? "Expired" : "Active"}>
            <TableCell sx={{ color: isInactive(grant) ? "#d90900" : "inherit" }}>
              {grant?.expiryTime.format(fmtConcise)}
            </TableCell>
          </Tooltip>
          <TableCell>{linkUser(grant?.grantedUser)}</TableCell>
          <TableCell align="right">
            <Tooltip arrow title={isInactive(grant) ? "Already canceled" : "Cancel"}>
              <span>
                <IconButton disabled={isInactive(grant)} onClick={() => setDeleting(true)}>
                  <CancelIcon />
                </IconButton>
              </span>
            </Tooltip>
          </TableCell>
          <Dialog open={deleting} onClose={() => setDeleting(false)}>
            <DialogTitle>Confirm Cancellation</DialogTitle>
            <DialogContent>
              <DialogContentText>Are you sure you want to cancel the grant?</DialogContentText>
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

export default TimedGrantRow;

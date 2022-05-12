import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Divider from "@mui/material/Divider";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { CaseMessage } from "../../util/API/APITypes";
import MoreVertIcon from "@mui/icons-material/MoreVert";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import IconButton from "@mui/material/IconButton";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import ListItemIcon from "@mui/material/ListItemIcon";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import TextField from "@mui/material/TextField";
import LoadingButton from "@mui/lab/LoadingButton";
import { linkUser } from "../../util/StringUtil";

interface Props {
  message: CaseMessage;
  readonly?: boolean;
  refreshHandler: () => void;
}

const CaseMessageItem = ({ message, readonly = false, refreshHandler }: Props) => {
  const [menuAnchor, setMenuAnchor] = useState<HTMLElement | null>(null);

  const [attemptingDelete, setAttemptingDelete] = useState(false);
  const [editing, setEditing] = useState(false);
  const [content, setContent] = useState(message.content);
  const [isPending, setIsPending] = useState(false);

  const handleDelete = async () => {
    await APIClient.cases.messages.delete(message.id);
    setAttemptingDelete(false);
    refreshHandler();
  };

  const handleDeleteAttempt = () => {
    handleCloseMenu();
    setAttemptingDelete(true);
  };

  const handleCloseMenu = () => {
    setMenuAnchor(null);
  };

  const handleEditAttempt = () => {
    handleCloseMenu();
    setEditing(true);
  };

  const handleEdit = async () => {
    setIsPending(true);
    await APIClient.cases.messages.update(message.id, { content });
    refreshHandler();
    setIsPending(false);
    setEditing(false);
  };

  const handleCancelEdit = () => {
    setContent(message.content);
    setEditing(false);
  };

  const validate = () => 0 < content.length && content.length < 2000;

  return (
    <Box>
      <Stack direction="row" py={1}>
        <Box sx={{ width: "100%" }}>
          <Typography variant="caption" gutterBottom>
            <strong>{linkUser(message.user)}</strong> on {message.dateTime.format("D MMM YYYY [at] H:mm")}
          </Typography>
          {editing ? (
            <>
              <TextField
                id={`case-message-edit-input-${message.id.substring(0, 8)}`}
                label="Message"
                variant="filled"
                multiline
                fullWidth
                margin="dense"
                minRows={4}
                value={content}
                onChange={(e) => setContent(e.target.value)}
              />
              <Stack direction="row" justifyContent="end">
                <Button variant="text" onClick={handleCancelEdit}>
                  Cancel
                </Button>
                <LoadingButton
                  loading={isPending}
                  disabled={!validate()}
                  onClick={handleEdit}
                  variant="text"
                  loadingPosition="center"
                >
                  Save
                </LoadingButton>
              </Stack>
            </>
          ) : (
            <Typography variant="body2" sx={{ whiteSpace: "pre-wrap" }} gutterBottom>
              {message.content}
            </Typography>
          )}
        </Box>
        {editing || readonly ? null : (
          <Stack justifyContent="start" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <IconButton onClick={(e) => setMenuAnchor(e.currentTarget)}>
              <MoreVertIcon />
            </IconButton>
          </Stack>
        )}
      </Stack>
      <Divider />
      <Menu open={Boolean(menuAnchor)} onClose={handleCloseMenu} anchorEl={menuAnchor}>
        <MenuItem onClick={handleEditAttempt}>
          <ListItemIcon>
            <EditIcon fontSize="small" />
          </ListItemIcon>{" "}
          Edit
        </MenuItem>
        <MenuItem onClick={handleDeleteAttempt}>
          <ListItemIcon>
            <DeleteIcon fontSize="small" />
          </ListItemIcon>{" "}
          Delete
        </MenuItem>
      </Menu>
      <Dialog open={attemptingDelete} onClose={() => setAttemptingDelete(false)}>
        <DialogTitle>Confirm Message Deletion</DialogTitle>
        <DialogContent>
          <DialogContentText>Are you sure you want to delete this message?</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setAttemptingDelete(false)}>No</Button>
          <Button onClick={handleDelete} autoFocus>
            Yes
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};

export default CaseMessageItem;

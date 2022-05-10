import React, { useState } from "react";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import TextField from "@mui/material/TextField";
import LoadingButton from "@mui/lab/LoadingButton";
import Typography from "@mui/material/Typography";
import APIClient from "../../util/API/APIClient";
import Stack from "@mui/material/Stack";
import { Announcement } from "../../util/API/APITypes";
import Button from "@mui/material/Button";

interface Props {
  data?: Announcement;
  changeHandler: () => void;
  cancelHandler?: () => void;
}

const AnnouncementItemEditable = ({ data, changeHandler, cancelHandler }: Props) => {
  const editing = data !== undefined;

  const [title, setTitle] = useState(data?.title || "");
  const [message, setMessage] = useState(data?.message || "");
  const [isPending, setIsPending] = useState(false);

  const validateAnnouncement = () => title.length > 0 && message.length > 0;

  const handleCreateAnnouncement = async () => {
    setIsPending(true);
    await APIClient.announcements.create({ title, message });
    setTitle("");
    setMessage("");
    setIsPending(false);
    changeHandler();
  };

  const handleEditAnnouncement = async () => {
    setIsPending(true);
    if (data === undefined) throw Error("No existing announcement");
    await APIClient.announcements.update(data.id, { title, message });
    setIsPending(false);
    changeHandler();
  };

  const handleCancel = () => {
    setTitle("");
    setMessage("");
    if (cancelHandler) cancelHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        {editing ? null : (
          <Typography variant="h5" gutterBottom>
            Create Announcement
          </Typography>
        )}

        <TextField
          id="announcement-title-input"
          label="Title"
          variant="filled"
          fullWidth
          margin="dense"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
        />
        <TextField
          id="announcement-message-input"
          label="Message"
          variant="filled"
          multiline
          fullWidth
          margin="dense"
          minRows={4}
          value={message}
          onChange={(e) => setMessage(e.target.value)}
        />
        <Stack direction="row" justifyContent="end">
          <Button variant="text" onClick={handleCancel}>
            Cancel
          </Button>
          <LoadingButton
            loading={isPending}
            disabled={!validateAnnouncement()}
            onClick={editing ? handleEditAnnouncement : handleCreateAnnouncement}
            variant="text"
            loadingPosition="center"
          >
            {editing ? "Edit" : "Create"}
          </LoadingButton>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default AnnouncementItemEditable;

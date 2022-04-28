import React, { useState } from "react";
import { Announcement, UserType } from "../../util/API/APITypes";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import AnnouncementItemEditable from "./AnnouncementItemEditable";
import CardActions from "@mui/material/CardActions";
import IconButton from "@mui/material/IconButton";
import EditIcon from "@mui/icons-material/Edit";
import DeleteIcon from "@mui/icons-material/Delete";
import { useUserContext } from "../UserContext";
import Stack from "@mui/material/Stack";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import Button from "@mui/material/Button";
import DialogActions from "@mui/material/DialogActions";
import APIClient from "../../util/API/APIClient";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import Tooltip from "@mui/material/Tooltip";

interface Props {
  readonly?: boolean;
  data: Announcement;
  className?: string;
  changeHandler: () => void;
}

const AnnouncementItem = ({ readonly = false, data, className, changeHandler }: Props) => {
  const [editing, setEditing] = useState(false);
  const [deleting, setDeleting] = useState(false);
  const { user } = useUserContext();

  const cancelEditHandler = () => {
    setEditing(false);
  };

  const handleDelete = async () => {
    setDeleting(false);
    if (data.id === undefined) throw Error("No ID for announcement deletion");
    await APIClient.announcements.delete(data.id);
    changeHandler();
  };

  const changeHandlerIntermediate = () => {
    changeHandler();
    setEditing(false);
  };

  return (
    <>
      {editing ? (
        <AnnouncementItemEditable
          data={data}
          changeHandler={changeHandlerIntermediate}
          cancelHandler={cancelEditHandler}
        />
      ) : (
        <Card className={`max-w-screen-md ${className}`}>
          <CardContent>
            <Typography variant="h5">{data.title}</Typography>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              {data.dateTime?.toString()}
            </Typography>
            <Typography variant="body1" sx={{ whiteSpace: "pre-wrap" }} >{data.message}</Typography>
          </CardContent>
          {readonly || user?.userType === UserType.Patient ? null : (
            <CardActions>
              <Stack className="w-full" direction="row" justifyContent="end">
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
              </Stack>
              <Dialog open={deleting} onClose={() => setDeleting(false)}>
                <DialogTitle>Confirm Deletion</DialogTitle>
                <DialogContent>
                  <DialogContentText>Are you sure you want to delete this announcement?</DialogContentText>
                </DialogContent>
                <DialogActions>
                  <Button onClick={() => setDeleting(false)}>No</Button>
                  <Button onClick={handleDelete} autoFocus>
                    Yes
                  </Button>
                </DialogActions>
              </Dialog>
            </CardActions>
          )}
        </Card>
      )}
    </>
  );
};

export default AnnouncementItem;

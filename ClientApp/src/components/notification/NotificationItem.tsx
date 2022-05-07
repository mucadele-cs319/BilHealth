import Card from "@mui/material/Card";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import IconButton from "@mui/material/IconButton";
import Stack from "@mui/material/Stack";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";
import React from "react";
import { Notification } from "../../util/API/APITypes";
import DoneIcon from "@mui/icons-material/Done";
import APIClient from "../../util/API/APIClient";

interface Props {
  data: Notification;
}

const NotificationItem = ({ data: notification }: Props) => {
  const handleRead = async () => {
    await APIClient.notifications.markRead(notification.id);
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h6">{notification.type}</Typography>
        <Typography variant="subtitle2" color="text.secondary" gutterBottom>
          {notification.dateTime?.format("dddd, D MMM  YYYY [at] H:mm")}
        </Typography>
        <Typography variant="body1" sx={{ whiteSpace: "pre-wrap" }}>
          Some kind of description for this notification item.
        </Typography>
      </CardContent>
      <CardActions>
        <Stack className="w-full" direction="row" justifyContent="end">
          <Tooltip arrow title="Mark Read">
            <IconButton disabled={notification.read} onClick={handleRead}>
              <DoneIcon />
            </IconButton>
          </Tooltip>
        </Stack>
      </CardActions>
    </Card>
  );
};

export default NotificationItem;

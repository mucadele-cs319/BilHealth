import Card from "@mui/material/Card";
import CardActions from "@mui/material/CardActions";
import CardContent from "@mui/material/CardContent";
import IconButton from "@mui/material/IconButton";
import Stack from "@mui/material/Stack";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";
import React from "react";
import DoneIcon from '@mui/icons-material/Done';
import APIClient from "../../util/API/APIClient";

const NotificationControlCard = () => {

  const handleRead = async () => {
    await APIClient.notifications.markAllRead();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5">Notifications</Typography>
        <Typography variant="body1" sx={{ whiteSpace: "pre-wrap" }}>
          You can find and navigate your notifications on this page.
        </Typography>
      </CardContent>
      <CardActions>
        <Stack className="w-full" direction="row" justifyContent="end">
          <Tooltip arrow title="Mark All Read">
            <IconButton onClick={handleRead}>
              <DoneIcon />
            </IconButton>
          </Tooltip>
        </Stack>
      </CardActions>
    </Card>
  );
};

export default NotificationControlCard;

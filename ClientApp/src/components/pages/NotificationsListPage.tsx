import CircularProgress from "@mui/material/CircularProgress";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Notification } from "../../util/API/APITypes";
import { useDocumentTitle } from "../../util/CustomHooks";
import NotificationControlCard from "../notification/NotificationControlCard";
import NotificationItem from "../notification/NotificationItem";

const NotificationsListPage = () => {
  useDocumentTitle("Notifications");

  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);

  const refreshNotifications = () => {
    APIClient.notifications.get().then((response) => {
      setNotifications(response);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshNotifications();
  }, []);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          <NotificationControlCard refreshHandler={refreshNotifications} />
          {isLoaded ? (
            notifications.length !== 0 ? (
              notifications.map((notification, i) => (
                <NotificationItem key={i} notification={notification} refreshHandler={refreshNotifications} />
              ))
            ) : (
              <Stack alignItems="center" className="mt-8">
                <Typography color="text.secondary">No notifications at this time.</Typography>
              </Stack>
            )
          ) : (
            <Stack alignItems="center" className="mt-8">
              <CircularProgress />
            </Stack>
          )}
        </Grid>
      </Fade>
    </Grid>
  );
};

export default NotificationsListPage;

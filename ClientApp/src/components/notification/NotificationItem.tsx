import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import IconButton from "@mui/material/IconButton";
import Stack from "@mui/material/Stack";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";
import React from "react";
import { Notification, NotificationType, stringifyNotificationType } from "../../util/API/APITypes";
import DoneIcon from "@mui/icons-material/Done";
import APIClient from "../../util/API/APIClient";
import Box from "@mui/material/Box";
import Link from "@mui/material/Link";
import { Link as RLink } from "react-router-dom";

const describeNotification = ({ type, referenceId1, userId }: Notification) => {
  const linkify = (str: string, to: string) => <Link component={RLink} to={to}>{str}</Link>;
  const cLink = (caseId?: string) => `/cases/${caseId}`;
  const tLink = () => `/profiles/${userId}/test-results`;

  switch (type) {
    case NotificationType.CaseNewAppointment:
      return <>A new appointment has been created for {linkify("your case", cLink(referenceId1))}.</>;
    case NotificationType.CaseAppointmentTimeChanged:
      return <>The appointment on {linkify("your case", cLink(referenceId1))} has had its time changed.</>;
    case NotificationType.CaseAppointmentCanceled:
      return <>The appointment on {linkify("your case", cLink(referenceId1))} has been canceled.</>;
    case NotificationType.CaseNewMessage:
      return <>A new message was sent to {linkify("your case", cLink(referenceId1))}.</>;
    case NotificationType.CaseClosed:
      return <>{linkify("Your case", cLink(referenceId1))} has been closed.</>;
    case NotificationType.CaseTriaged:
      return <>{linkify("Your case", cLink(referenceId1))} has been triaged.</>;
    case NotificationType.CaseDoctorChanged:
      return <>{linkify("Your case", cLink(referenceId1))} has had its doctor unassigned.</>;
    case NotificationType.CaseNewPrescription:
      return <>A new prescription was added to {linkify("your case", cLink(referenceId1))}.</>;
    case NotificationType.TestResultNew:
      return <>A {linkify("new test result", tLink())} has been added for you.</>;
  }
};

interface Props {
  notification: Notification;
  refreshHandler: () => void;
}

const NotificationItem = ({ notification, refreshHandler }: Props) => {
  const handleRead = async () => {
    await APIClient.notifications.markRead(notification.id);
    refreshHandler();
  };

  return (
    <Card
      className={`max-w-screen-md mb-1 mx-auto ${
        notification.read ? "opacity-70 hover:opacity-100" : "border-r-8 border-blue-400"
      }`}
      sx={{ transition: "all .3s" }}
    >
      <CardContent>
        <Stack direction="row">
          <Box>
            <Typography variant="h6">{stringifyNotificationType(notification.type)}</Typography>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              {notification.dateTime?.format("dddd, D MMM YYYY [at] H:mm")}
            </Typography>
            <Typography variant="body1" sx={{ whiteSpace: "pre-wrap" }}>
              {describeNotification(notification)}
            </Typography>
          </Box>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            {notification.read ? null : (
              <Tooltip arrow title="Mark Read">
                <IconButton disabled={notification.read} onClick={handleRead}>
                  <DoneIcon fontSize="large" />
                </IconButton>
              </Tooltip>
            )}
          </Stack>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default NotificationItem;

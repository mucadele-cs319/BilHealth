import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const NotificationsListPage = () => {
  useDocumentTitle("Notifications");

  return <div id="notifications-list-container"></div>;
};

export default NotificationsListPage;

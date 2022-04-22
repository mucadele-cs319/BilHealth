import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Announcement } from "../../util/API/APITypes";
import AnnouncementItem from "../AnnouncementItem";

const Announcements = (): JSX.Element => {
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);

  useEffect(() => {
    APIClient.announcements.get().then((response) => {
      setAnnouncements(response);
    });
  }, []);

  return (
    <div id="announcements-container">
      <main id="announcements-list">
        {announcements.length !== 0 ? (
          announcements.map((announcement, i) => <AnnouncementItem key={i} data={announcement} />)
        ) : (
          <span>No announcements at this time.</span>
        )}
      </main>
    </div>
  );
};

export default Announcements;

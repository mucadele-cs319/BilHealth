import Box from "@mui/material/Box";
import CircularProgress from "@mui/material/CircularProgress";
import React, { useEffect, useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Announcement } from "../../util/API/APITypes";
import AnnouncementItem from "../AnnouncementItem";

const Announcements = () => {
  const [announcements, setAnnouncements] = useState<Announcement[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);

  useEffect(() => {
    APIClient.announcements.get().then((response) => {
      setAnnouncements(response);
      setIsLoaded(true);
    });
  }, []);

  return (
    <Box>
      <Box className="mx-auto w-fit max-w-screen-md">
        {isLoaded ? (
          announcements.length !== 0 ? (
            announcements.map((announcement, i) => <AnnouncementItem key={i} data={announcement} className="mb-5" />)
          ) : (
            <span>No announcements at this time.</span>
          )
        ) : (
          <CircularProgress className="mt-8" />
        )}
      </Box>
    </Box>
  );
};

export default Announcements;

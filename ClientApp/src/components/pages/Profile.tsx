import React from "react";
import Box from "@mui/material/Box";
import { useUserContext } from "../UserContext";

const Profile = () => {
  const { user } = useUserContext();

  return (
    <Box>
      Temporarily shows signed-in user profile:
      <pre>
        <code>{JSON.stringify(user, null, 2)}</code>
      </pre>
    </Box>
  );
};

export default Profile;

import React from "react";
import Box from "@mui/material/Box";
import { useUserContext } from "../UserContext";
import { useDocumentTitle } from "../../util/CustomHooks";

const Profile = () => {
  const { user } = useUserContext();
  useDocumentTitle(`${user?.firstName}'s Profile`);

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

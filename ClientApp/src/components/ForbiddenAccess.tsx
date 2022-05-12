import React from "react";
import WarningAmberIcon from "@mui/icons-material/WarningAmber";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";

const ForbiddenAccess = () => {
  return (
    <Stack alignItems="center" justifyContent="center" mt={5} height="50vh">
      <Typography color="text.secondary">You are not allowed to view this page.</Typography>
      <WarningAmberIcon sx={{ mt: "16px", fontSize: "10em" }} color="error" fontSize="large" />
    </Stack>
  );
};

export default ForbiddenAccess;

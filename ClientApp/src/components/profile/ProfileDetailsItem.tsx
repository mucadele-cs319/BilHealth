import Divider from "@mui/material/Divider";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React from "react";

interface Props {
  title: string;
  content: string;
}

const ProfileDetailsItem = ({ title, content }: Props) => {
  return (
    <Stack>
      <Typography variant="caption">{title}</Typography>
      <Divider />
      <Typography variant="body2" sx={{ mt: 1 }}>
        {content}
      </Typography>
    </Stack>
  );
};

export default ProfileDetailsItem;

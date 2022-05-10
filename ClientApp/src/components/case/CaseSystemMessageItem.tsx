import Box from "@mui/material/Box";
import Typography from "@mui/material/Typography";
import React from "react";
import { CaseSystemMessage } from "../../util/API/APITypes";
import Divider from "@mui/material/Divider";

interface Props {
  message: CaseSystemMessage;
}

const CaseSystemMessageItem = ({ message }: Props) => {
  return (
    <Box>
      <Typography variant="caption" gutterBottom>
        <strong>BilHealth</strong> on {message.dateTime.format("D MMM  YYYY [at] H:mm")}
      </Typography>
      <Typography component="p" variant="overline" gutterBottom>
        {message.content}
      </Typography>
      <Divider />
    </Box>
  );
};

export default CaseSystemMessageItem;

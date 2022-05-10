import Box from "@mui/material/Box";
import Divider from "@mui/material/Divider";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import APIClient from "../../util/API/APIClient";
import { CaseMessage, SimpleUser } from "../../util/API/APITypes";

interface Props {
  message: CaseMessage;
}

const CaseMessageItem = ({ message }: Props) => {
  const [author, setAuthor] = useState<SimpleUser>();
  const [isLoaded, setIsLoaded] = useState(false);

  const getAuthor = async () => {
    APIClient.profiles.getSimple(message.userId).then((user) => {
      setAuthor(user);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    getAuthor();
  }, []);

  // TODO: EDIT AND DELETE BUTTONS
  // TODO: DONT FETCH FOR EVERY COMPONENT
  return (
    <Box>
      <Typography variant="caption" gutterBottom>
        <Link style={{ fontWeight: "bold" }} to={`/profiles/${author?.id}`}>
          {isLoaded ? `${author?.firstName} ${author?.lastName}` : "loading user"}
        </Link>{" "}
        on {message.dateTime.format("D MMM  YYYY [at] H:mm")}
      </Typography>
      <Typography variant="body2" gutterBottom>
        {message.content}
      </Typography>
      <Divider />
    </Box>
  );
};

export default CaseMessageItem;

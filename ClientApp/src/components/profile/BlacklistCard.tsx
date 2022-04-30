import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";

interface Props {
  patientId: string;
  blacklisted: boolean;
  refreshHandler: () => void;
}

const BlacklistCard = ({ patientId, blacklisted, refreshHandler }: Props) => {
  const [isPending, setIsPending] = useState(false);

  const toggleBlacklist = async () => {
    setIsPending(true);
    await APIClient.profiles.blacklist(patientId, !blacklisted);
    setIsPending(false);
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto border border-red-700">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Blacklist Details
        </Typography>
        <Typography variant="body2">
          This patient is currently
          <strong color={blacklisted ? "red" : "green"}>{blacklisted ? "blacklisted" : "not blacklisted"}</strong> from
          requesting appointments.
        </Typography>

        <Stack direction="row" justifyContent="end">
          <LoadingButton
            loading={isPending}
            onClick={toggleBlacklist}
            variant="text"
            loadingPosition="center"
            color={blacklisted ? "primary" : "error"}
          >
            {blacklisted ? "Unblacklist" : "Blacklist"}
          </LoadingButton>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default BlacklistCard;

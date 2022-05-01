import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";

const PasswordCard = () => {
  const [currentPassword, setCurrentPassword] = useState("");
  const [newPassword1, setNewPassword1] = useState("");
  const [newPassword2, setNewPassword2] = useState("");
  const [isPending, setIsPending] = useState(false);

  const handlePasswordChange = async () => {
    setIsPending(true);
    await APIClient.authentication.changePassword(currentPassword, newPassword1);
    setCurrentPassword("");
    setNewPassword1("");
    setNewPassword2("");
    setIsPending(false);
  };

  const validate = () =>
    newPassword1 === newPassword2 &&
    newPassword1.length >= 6 &&
    /[a-zA-Z]/.test(newPassword1) &&
    /[0-9]/.test(newPassword2);

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Change Password
        </Typography>

        <Stack className="w-fit">
          <TextField
            id="changepw-pw-1-input"
            label="Current Password"
            variant="outlined"
            margin="dense"
            value={currentPassword}
            type="password"
            onChange={(e) => setCurrentPassword(e.target.value)}
          />
          <TextField
            id="changepw-pw-2-input"
            label="New Password"
            variant="outlined"
            margin="dense"
            value={newPassword1}
            type="password"
            onChange={(e) => setNewPassword1(e.target.value)}
          />
          <TextField
            id="changepw-pw-3-input"
            label="New Password (confirm)"
            variant="outlined"
            margin="dense"
            value={newPassword2}
            type="password"
            onChange={(e) => setNewPassword2(e.target.value)}
          />
        </Stack>

        <Stack direction="row" justifyContent="end">
          <LoadingButton
            disabled={!validate()}
            loading={isPending}
            onClick={handlePasswordChange}
            variant="text"
            loadingPosition="center"
          >
            Change Password
          </LoadingButton>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default PasswordCard;

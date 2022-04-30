import React, { useState } from "react";
import Stack from "@mui/material/Stack";
import APIClient from "../../util/API/APIClient";
import Card from "@mui/material/Card";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Button from "@mui/material/Button";
import LoadingButton from "@mui/lab/LoadingButton";
import CardContent from "@mui/material/CardContent";
import { getAllUserTypes, UserType } from "../../util/API/APITypes";
import MenuItem from "@mui/material/MenuItem";
import Grid from "@mui/material/Grid";
import Snackbar from "@mui/material/Snackbar";
import Alert, { AlertColor } from "@mui/material/Alert";

const RegistrationCard = () => {
  const [isPending, setIsPending] = useState(false);

  const [snackbar, setSnackbar] = useState(false);
  const [snackbarSeverity, setSnackbarSeverity] = useState<AlertColor>();
  const handleSnackbar = (severity: AlertColor | "none") => {
    if (severity === "none") {
      setSnackbar(false);
      setSnackbarSeverity(undefined);
    } else {
      setSnackbar(true);
      setSnackbarSeverity(severity);
    }
  };

  const [userType, setUserType] = useState("");
  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [email, setEmail] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");

  const handleCancel = () => {
    setUserType("");
    setUserName("");
    setPassword("");
    setEmail("");
    setFirstName("");
    setLastName("");
  };

  const handleRegister = async () => {
    setIsPending(true);
    try {
      await APIClient.authentication.register({
        userType,
        userName,
        password,
        email,
        firstName,
        lastName,
      });
    } catch (error) {
      console.warn(error);
      setIsPending(false);
      handleSnackbar("error");
      return;
    }

    handleCancel();
    setIsPending(false);
    handleSnackbar("success");
  };

  const validateForm = () => {
    return (
      userType.length > 0 &&
      userName.length > 3 &&
      password.length >= 6 &&
      /[a-zA-Z]/.test(password) &&
      /[0-9]/.test(password) &&
      email.length > 5 &&
      firstName.length > 0 &&
      lastName.length > 0
    );
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Register User
        </Typography>

        <Grid container justifyContent="center" spacing={2}>
          <Grid item sm={4} sx={{ width: { xs: "50%" } }}>
            <TextField
              select
              margin="dense"
              fullWidth
              label="User Type"
              onChange={(e) => setUserType(e.target.value)}
              value={userType}
            >
              {getAllUserTypes()
                .filter((type) => type !== UserType.Admin)
                .map((type) => (
                  <MenuItem key={type} value={type}>
                    {type}
                  </MenuItem>
                ))}
            </TextField>
          </Grid>
          <Grid item sm={4}>
            <TextField
              id="registration-username-input"
              label="Username"
              variant="outlined"
              margin="dense"
              value={userName}
              onChange={(e) => setUserName(e.target.value)}
              helperText="Should be an ID / Student ID"
            />
          </Grid>
          <Grid item sm={4}>
            <TextField
              id="registration-password-input"
              label="Password"
              variant="outlined"
              margin="dense"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              helperText="Over 6 chars, at least 1 digit"
            />
          </Grid>
          <Grid item sm={4}>
            <TextField
              id="registration-email-input"
              label="Email"
              variant="outlined"
              margin="dense"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
            />
          </Grid>
          <Grid item sm={4}>
            <TextField
              id="registration-firstname-input"
              label="First Name"
              variant="outlined"
              margin="dense"
              value={firstName}
              onChange={(e) => setFirstName(e.target.value)}
            />
          </Grid>
          <Grid item sm={4}>
            <TextField
              id="registration-lastname-input"
              label="Last Name"
              variant="outlined"
              margin="dense"
              value={lastName}
              onChange={(e) => setLastName(e.target.value)}
            />
          </Grid>
        </Grid>

        <Stack direction="row" justifyContent="end">
          <Button variant="text" onClick={handleCancel}>
            Cancel
          </Button>
          <LoadingButton
            loading={isPending}
            disabled={!validateForm()}
            onClick={handleRegister}
            variant="text"
            loadingPosition="center"
          >
            Register
          </LoadingButton>
        </Stack>
      </CardContent>
      <Snackbar
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        open={snackbar}
        autoHideDuration={10000}
        onClose={() => {
          handleSnackbar("none");
        }}
      >
        <Alert severity={snackbarSeverity}>
          {snackbarSeverity === "success" ? "Registered user successfully!" : "Failed to register user!"}
        </Alert>
      </Snackbar>
    </Card>
  );
};

export default RegistrationCard;

import React, { FormEvent, useState } from "react";
import AnnouncementList from "./AnnouncementList";
import Paper from "@mui/material/Paper";
import Typography from "@mui/material/Typography";
import TextField from "@mui/material/TextField";
import Checkbox from "@mui/material/Checkbox";
import FormControlLabel from "@mui/material/FormControlLabel";
import FormGroup from "@mui/material/FormGroup";
import LoginIcon from "@mui/icons-material/Login";
import LoadingButton from "@mui/lab/LoadingButton";
import { useNavigate } from "react-router-dom";
import { useUserContext } from "../UserContext";
import { useDocumentTitle } from "../../util/CustomHooks";
import Grid from "@mui/material/Grid";

const LoginPage = () => {
  useDocumentTitle("Login");
  const navigate = useNavigate();

  const { login } = useUserContext();

  const [error, setError] = useState(false);
  const [isLoading, setIsLoading] = useState(false);

  const [userName, setUserName] = useState("");
  const [password, setPassword] = useState("");
  const [rememberMe, setRememberMe] = useState(false);

  const validate = () => userName.length > 2 && password.length > 4;

  const handleLogin = async (event: FormEvent) => {
    event.preventDefault();

    setError(false);
    setIsLoading(true);
    if (await login({ userName, password, rememberMe })) navigate("/");
    else {
      setError(true);
      setIsLoading(false);
    }
  };

  return (
    <Grid container>
      <Grid item xs={12} sm={6} className="h-screen py-10 overflow-auto" sx={{ scrollbarWidth: "thin" }}>
        <Typography variant="h4" gutterBottom textAlign="center">
          Announcements
        </Typography>
        <AnnouncementList readonly />
      </Grid>
      <Grid item xs={12} sm={6} container justifyContent="center" alignItems="center" className="h-screen bg-cyan-500 shadow-lg shadow-black">
        <Grid item xs={10} lg={5}>
          <Paper elevation={8} className="p-5 h-fit max-w-sm mx-auto">
            <Typography variant="h5" gutterBottom>
              BilHealth - Login
            </Typography>
            <form onSubmit={handleLogin}>
              <TextField
                id="input-username"
                label="Username"
                variant="outlined"
                margin="normal"
                fullWidth
                onChange={(e) => setUserName(e.target.value)}
                error={error}
              />
              <TextField
                id="input-password"
                type="password"
                label="Password"
                variant="outlined"
                margin="normal"
                fullWidth
                onChange={(e) => setPassword(e.target.value)}
                error={error}
              />
              <FormGroup>
                <FormControlLabel
                  control={<Checkbox onChange={(e) => setRememberMe(e.target.checked)} />}
                  label="Remember me"
                />
              </FormGroup>
              <LoadingButton
                loading={isLoading}
                disabled={!validate()}
                variant="contained"
                loadingPosition="start"
                fullWidth
                startIcon={<LoginIcon />}
                type="submit"
              >
                Login
              </LoadingButton>
            </form>
          </Paper>
        </Grid>
      </Grid>
    </Grid>
  );
};

export default LoginPage;

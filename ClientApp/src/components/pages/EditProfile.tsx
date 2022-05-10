import DatePicker from "@mui/lab/DatePicker";
import LocalizationProvider from "@mui/lab/LocalizationProvider";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import CircularProgress from "@mui/material/CircularProgress";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import MenuItem from "@mui/material/MenuItem";
import Stack from "@mui/material/Stack";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import { Dayjs } from "dayjs";
import React, { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import APIClient from "../../util/API/APIClient";
import {
  BloodType,
  Campus,
  Gender,
  getAllBloodTypes,
  getAllCampusTypes,
  getAllGenderTypes,
  stringifyBloodType,
  stringifyCampus,
  stringifyGender,
  User,
  UserType,
} from "../../util/API/APITypes";
import { titleify, useDocumentTitle } from "../../util/CustomHooks";
import { useUserContext } from "../UserContext";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import Divider from "@mui/material/Divider";
import LoadingButton from "@mui/lab/LoadingButton";
import InputAdornment from "@mui/material/InputAdornment";

const EditProfile = () => {
  useDocumentTitle("Editing Profile");
  const navigate = useNavigate();

  const { user } = useUserContext();
  const fullEditing = [UserType.Staff, UserType.Admin].some((type) => type === user?.userType);

  const params = useParams();
  const [queryUser, setQueryUser] = useState<User>();
  const [isLoaded, setIsLoaded] = useState(false);
  const [isSavePending, setIsSavePending] = useState(false);

  const [gender, setGender] = useState<number>(Gender.Unspecified);
  const [dateOfBirth, setDateOfBirth] = useState<Dayjs | null>(null);
  const [bodyWeight, setBodyWeight] = useState<number | undefined>();
  const [bodyHeight, setBodyHeight] = useState<number | undefined>();
  const [bloodType, setBloodType] = useState<number>(BloodType.Unspecified);
  const [specialization, setSpecialization] = useState<string | undefined>();
  const [campus, setCampus] = useState(Campus.Main);

  const refreshUser = () => {
    if (params.userid === undefined) throw Error("Couldn't get URL param for user ID");
    APIClient.profiles.get(params.userid).then((fetchedUser) => {
      setQueryUser(fetchedUser);
      setGender(fetchedUser.gender || Gender.Unspecified);
      setDateOfBirth(fetchedUser.dateOfBirth || null);
      setBodyWeight(fetchedUser.bodyWeight);
      setBodyHeight(fetchedUser.bodyHeight);
      setBloodType(fetchedUser.bloodType || BloodType.Unspecified);
      setSpecialization(fetchedUser.specialization);
      setCampus(fetchedUser.campus || Campus.Main);

      document.title = titleify(`Editing ${fetchedUser.firstName}'s Profile`);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshUser();
  }, [params]);

  const handleSave = async () => {
    setIsSavePending(true);
    if (queryUser === undefined) throw Error("User not loaded yet");
    await APIClient.profiles.update(queryUser.id, {
      gender,
      dateOfBirth: dateOfBirth || undefined,
      bodyWeight,
      bodyHeight,
      bloodType,
      specialization,
      campus,
    });
    setIsSavePending(false);
    navigate("./..");
  };

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {isLoaded && queryUser ? (
            <Card className="max-w-screen-md mb-5 mx-auto">
              <CardContent>
                <Stack direction="row" justifyContent="center">
                  <Typography variant="h5" gutterBottom>
                    {`Editing ${queryUser.firstName} ${queryUser.lastName}'s profile`}
                  </Typography>
                  <Stack direction="row" justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
                    <Button onClick={() => navigate("./..")} variant="text">
                      Cancel
                    </Button>
                    <LoadingButton loading={isSavePending} onClick={handleSave} variant="text">
                      Save
                    </LoadingButton>
                  </Stack>
                </Stack>

                <Box mt={1}>
                  <Typography variant="body2" gutterBottom>
                    These details are only modifiable by health center staff.
                  </Typography>
                  <Grid container spacing={2}>
                    <Grid item>
                      <TextField
                        disabled={!fullEditing}
                        select
                        margin="dense"
                        fullWidth
                        label="Gender"
                        onChange={(e) => setGender(parseInt(e.target.value))}
                        value={gender}
                      >
                        {getAllGenderTypes().map((type) => (
                          <MenuItem key={type} value={type}>
                            {stringifyGender(type)}
                          </MenuItem>
                        ))}
                      </TextField>
                    </Grid>
                    <Grid item my={1}>
                      <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DatePicker
                          disabled={!fullEditing}
                          inputFormat="DD/MM/YYYY"
                          label="Date of Birth"
                          clearable={true}
                          value={dateOfBirth}
                          onChange={(date) => setDateOfBirth(date || null)}
                          renderInput={(params) => <TextField {...params} />}
                        />
                      </LocalizationProvider>
                    </Grid>
                  </Grid>
                </Box>

                {queryUser.userType !== UserType.Patient ? null : (
                  <Box mt={2}>
                    <Divider sx={{ mb: 1 }} />
                    <Typography variant="subtitle1">Patient Details</Typography>
                    <Grid container spacing={2}>
                      <Grid item>
                        <TextField
                          id="profile-edit-bw-input"
                          label="Body Weight"
                          variant="outlined"
                          margin="dense"
                          value={bodyWeight}
                          type="number"
                          inputProps={{
                            step: "0.5",
                          }}
                          InputProps={{
                            endAdornment: <InputAdornment position="end">kg</InputAdornment>,
                          }}
                          onChange={(e) => setBodyWeight(parseFloat(e.target.value))}
                        />
                      </Grid>
                      <Grid item>
                        <TextField
                          id="profile-edit-bh-input"
                          label="Body Height"
                          variant="outlined"
                          margin="dense"
                          value={bodyHeight}
                          type="number"
                          inputProps={{
                            step: "0.5",
                          }}
                          InputProps={{
                            endAdornment: <InputAdornment position="end">cm</InputAdornment>,
                          }}
                          onChange={(e) => setBodyHeight(parseFloat(e.target.value))}
                        />
                      </Grid>
                      <Grid item>
                        <TextField
                          select
                          margin="dense"
                          fullWidth
                          label="Blood Type"
                          onChange={(e) => setBloodType(parseInt(e.target.value))}
                          value={bloodType}
                        >
                          {getAllBloodTypes().map((type) => (
                            <MenuItem key={type} value={type}>
                              {stringifyBloodType(type)}
                            </MenuItem>
                          ))}
                        </TextField>
                      </Grid>
                    </Grid>
                  </Box>
                )}

                {queryUser.userType !== UserType.Doctor ? null : (
                  <Box mt={2}>
                    <Divider sx={{ mb: 1 }} />
                    <Typography variant="subtitle1">Doctor Details</Typography>
                    <Grid container spacing={2}>
                      <Grid item>
                        <TextField
                          id="profile-edit-s-input"
                          label="Specialization"
                          variant="outlined"
                          margin="dense"
                          value={specialization}
                          onChange={(e) => setSpecialization(e.target.value)}
                        />
                      </Grid>
                      <Grid item>
                        <TextField
                          select
                          margin="dense"
                          fullWidth
                          label="Campus"
                          onChange={(e) => setCampus(parseInt(e.target.value))}
                          value={campus}
                        >
                          {getAllCampusTypes().map((type) => (
                            <MenuItem key={type} value={type}>
                              {stringifyCampus(type)}
                            </MenuItem>
                          ))}
                        </TextField>
                      </Grid>
                    </Grid>
                  </Box>
                )}
              </CardContent>
            </Card>
          ) : (
            <Stack alignItems="center" className="mt-8">
              <CircularProgress />
            </Stack>
          )}
        </Grid>
      </Fade>
    </Grid>
  );
};

export default EditProfile;

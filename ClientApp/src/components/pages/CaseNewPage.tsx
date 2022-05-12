import React, { useState } from "react";
import { useDocumentTitle } from "../../util/CustomHooks";
import Grid from "@mui/material/Grid";
import Fade from "@mui/material/Fade";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import APIClient from "../../util/API/APIClient";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import { useUserContext } from "../UserContext";
import { CaseType, getAllCaseTypes, stringifyCaseType } from "../../util/API/APITypes";
import MenuItem from "@mui/material/MenuItem";
import TextField from "@mui/material/TextField";
import { isPatient } from "../../util/UserTypeUtil";
import { useNavigate } from "react-router-dom";
import LoadingButton from "@mui/lab/LoadingButton";

const CaseNewPage = () => {
  useDocumentTitle("Cases");

  const navigate = useNavigate();

  const { user } = useUserContext();

  const [isPending, setIsPending] = useState(false);

  const [caseType, setCaseType] = useState<number>(CaseType.Dental);
  const [title, setTitle] = useState("");
  const [patientUserId, setPatientUserId] = useState(isPatient(user) ? user.id : "");

  const handleCreate = async () => {
    setIsPending(true);

    const _case = await APIClient.cases.create({ title, patientUserId, type: caseType });
    navigate(`/cases/${_case.id}`);
  };

  const validate = () => title.length > 5 && patientUserId.length > 5;

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          <Card className="max-w-screen-md mb-5 mx-auto">
            <CardContent>
              <Typography variant="h5" gutterBottom sx={{ mb: 3 }}>
                Create New Case
              </Typography>

              <Grid container mt={1} justifyContent="center" spacing={2}>
                <Grid item sm={12}>
                  <TextField
                    id="case-title-input"
                    label="Title"
                    variant="outlined"
                    margin="dense"
                    fullWidth
                    value={title}
                    onChange={(e) => setTitle(e.target.value)}
                  />
                </Grid>
                <Grid item sm={4} sx={{ width: { xs: "50%" } }}>
                  <TextField
                    select
                    margin="dense"
                    fullWidth
                    label="Case Type"
                    onChange={(e) => setCaseType(parseInt(e.target.value))}
                    value={caseType}
                  >
                    {getAllCaseTypes().map((type) => (
                      <MenuItem key={type} value={type}>
                        {stringifyCaseType(type)}
                      </MenuItem>
                    ))}
                  </TextField>
                </Grid>
                <Grid item sm={4}>
                  <TextField
                    disabled={isPatient(user)}
                    id="case-userid-input"
                    label="Patient User ID"
                    variant="outlined"
                    margin="dense"
                    value={patientUserId}
                    onChange={(e) => setPatientUserId(e.target.value)}
                  />
                </Grid>
              </Grid>

              <Stack direction="row" justifyContent="end">
                <LoadingButton
                  loading={isPending}
                  disabled={!validate()}
                  onClick={handleCreate}
                  variant="text"
                  loadingPosition="center"
                >
                  Create
                </LoadingButton>
              </Stack>
            </CardContent>
          </Card>
        </Grid>
      </Fade>
    </Grid>
  );
};

export default CaseNewPage;

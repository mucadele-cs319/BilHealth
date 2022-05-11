import React, { useEffect, useState } from "react";
import { useDocumentTitle } from "../../util/CustomHooks";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Fade from "@mui/material/Fade";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import APIClient from "../../util/API/APIClient";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import { SimpleCase } from "../../util/API/APITypes";
import CaseItemCard from "../case/CaseItemCard";
import Button from "@mui/material/Button";
import { Link } from "react-router-dom";
import AddIcon from "@mui/icons-material/Add";

const CaseList = () => {
  useDocumentTitle("Cases");

  const [cases, setCases] = useState<SimpleCase[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);

  const refreshCases = () => {
    Promise.all([APIClient.cases.getList()]).then(([caseList]) => {
      setCases(caseList);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshCases();
  }, []);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          <Card className="max-w-screen-md mb-5 mx-auto">
            <CardContent>
              <Stack direction="row" sx={{ mb: 3 }}>
                <Typography variant="h5" gutterBottom>
                  Cases
                  <Typography variant="body2">
                    You are only able to view cases for which you have the required authorization.
                  </Typography>
                </Typography>
                <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
                  <Button component={Link} to="/cases/new" variant="text" startIcon={<AddIcon />}>
                    Create
                  </Button>
                </Stack>
              </Stack>

              {isLoaded ? (
                cases.length > 0 ? (
                  cases.map((_case) => <CaseItemCard key={_case.id} _case={_case} />)
                ) : (
                  <Stack alignItems="center" className="mt-8">
                    <Typography color="text.secondary">No viewable cases for you at this time.</Typography>
                  </Stack>
                )
              ) : (
                <Stack alignItems="center" className="mt-8">
                  <CircularProgress />
                </Stack>
              )}
            </CardContent>
          </Card>
        </Grid>
      </Fade>
    </Grid>
  );
};

export default CaseList;

import React, { useEffect, useState } from "react";
import { titleify, useDocumentTitle } from "../../util/CustomHooks";
import CircularProgress from "@mui/material/CircularProgress";
import Grid from "@mui/material/Grid";
import Fade from "@mui/material/Fade";
import Stack from "@mui/material/Stack";
import APIClient from "../../util/API/APIClient";
import { Case, UserType } from "../../util/API/APITypes";
import { useParams } from "react-router-dom";
import { useUserContext } from "../UserContext";
import CaseMessagesCard from "../case/CaseMessagesCard";
import CaseHeaderCard from "../case/CaseHeaderCard";
import TriageRequestCard from "../case/TriageRequestCard";
import PrescriptionCard from "../case/PrescriptionCard";
import AppointmentCard from "../case/AppointmentCard";

const CasePage = () => {
  useDocumentTitle("Case");

  const { user } = useUserContext();

  const params = useParams();

  const [_case, setCase] = useState<Case>();
  const [isLoaded, setIsLoaded] = useState(false);

  const refreshCase = () => {
    if (params.caseid === undefined) throw Error("No case ID?");
    Promise.all([APIClient.cases.get(params.caseid)]).then(([caseResponse]) => {
      setCase(caseResponse);
      document.title = titleify(caseResponse.title);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshCase();
  }, []);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {isLoaded && _case ? (
            <>
              <CaseHeaderCard _case={_case} refreshHandler={refreshCase} />
              <CaseMessagesCard _case={_case} refreshHandler={refreshCase} />
              <PrescriptionCard
                readonly={user?.userType !== UserType.Doctor}
                _case={_case}
                refreshHandler={refreshCase}
              />
              <AppointmentCard refreshHandler={refreshCase} _case={_case} />
              <TriageRequestCard _case={_case} refreshHandler={refreshCase} />
            </>
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

export default CasePage;

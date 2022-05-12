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
import ForbiddenAccess from "../ForbiddenAccess";

const CasePage = () => {
  useDocumentTitle("Case");

  const { user } = useUserContext();

  const params = useParams();

  const [_case, setCase] = useState<Case>();
  const [isLoaded, setIsLoaded] = useState(false);
  const [forbidden, setForbidden] = useState(false);
  const [isUnassignedDoctor, setIsUnassignedDoctor] = useState(true);

  const refreshCase = () => {
    if (params.caseid === undefined) throw Error("No case ID?");
    Promise.all([APIClient.cases.get(params.caseid)])
      .then(([caseResponse]) => {
        setCase(caseResponse);
        setIsUnassignedDoctor(user?.userType === UserType.Doctor && caseResponse.doctorUser?.id !== user.id);
        document.title = titleify(caseResponse.title);
        setIsLoaded(true);
      })
      .catch(() => {
        setForbidden(true);
      });
  };

  useEffect(() => {
    refreshCase();
  }, []);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {forbidden ? (
            <ForbiddenAccess />
          ) : isLoaded && _case ? (
            <>
              <CaseHeaderCard _case={_case} refreshHandler={refreshCase} readonly={isUnassignedDoctor} />
              <CaseMessagesCard _case={_case} refreshHandler={refreshCase} readonly={isUnassignedDoctor} />
              <PrescriptionCard
                readonly={user?.userType !== UserType.Doctor || isUnassignedDoctor}
                _case={_case}
                refreshHandler={refreshCase}
              />
              <AppointmentCard refreshHandler={refreshCase} _case={_case} />
              <TriageRequestCard _case={_case} refreshHandler={refreshCase} readonly={isUnassignedDoctor} />
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

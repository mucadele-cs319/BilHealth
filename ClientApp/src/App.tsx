import React from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import Announcements from "./components/pages/AnnouncementList";
import NotFound from "./components/pages/NotFound";
import CaseList from "./components/pages/CaseList";
import CasePage from "./components/pages/CasePage";
import NotificationsListPage from "./components/pages/NotificationsListPage";
import Profile from "./components/pages/Profile";
import TestResultList from "./components/pages/TestResultList";
import Box from "@mui/material/Box";
import LoginPage from "./components/pages/LoginPage";
import { useUserContext } from "./components/UserContext";
import CircularProgress from "@mui/material/CircularProgress";
import Typography from "@mui/material/Typography";
import { useMediaQuery, useTheme } from "@mui/material";
import FullLayout from "./components/FullLayout";
import StaffPanel from "./components/pages/StaffPanel";
import CalculatorsPage from "./components/pages/CalculatorsPage";
import EditProfile from "./components/pages/EditProfile";
import CaseNewPage from "./components/pages/CaseNewPage";

const LoadingPage = () => {
  return (
    <Box className="w-screen h-screen flex">
      <Box className="m-auto flex flex-col items-center">
        <CircularProgress />
        <Typography variant="overline" className="pt-2">
          Loading
        </Typography>
      </Box>
    </Box>
  );
};

const App = () => {
  const { user, loadingUser } = useUserContext();

  const theme = useTheme();
  const mobile = useMediaQuery(theme.breakpoints.down("md"));

  return (
    <>
      {loadingUser ? (
        <LoadingPage />
      ) : (
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route element={user !== null ? <FullLayout mobile={mobile} /> : <Navigate to="/login" />}>
            <Route path="/" element={<Announcements />} />
            <Route path="/cases" element={<CaseList />} />
            <Route path="/cases/new" element={<CaseNewPage />} />
            <Route path="/cases/:caseid" element={<CasePage />} />
            <Route path="/notifications" element={<NotificationsListPage />} />
            <Route path="/profiles/:userid" element={<Profile />} />
            <Route path="/profiles/:userid/edit" element={<EditProfile />} />
            <Route path="/profiles/:userid/test-results" element={<TestResultList />} />
            <Route path="/calculators" element={<CalculatorsPage />} />
            <Route path="/staff-panel" element={<StaffPanel />} />
            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      )}
    </>
  );
};

export default App;

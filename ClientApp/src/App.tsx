import React from "react";
import { Navigate, Outlet, Route, Routes } from "react-router-dom";
import Sidebar from "./components/Sidebar";
import Announcements from "./components/pages/AnnouncementList";
import NotFound from "./components/pages/NotFound";
import CaseList from "./components/pages/CaseList";
import CasePage from "./components/pages/CasePage";
import NotificationsListPage from "./components/pages/NotificationsListPage";
import Profile from "./components/pages/Profile";
import TestResultList from "./components/pages/TestResultList";
import TestResultPage from "./components/pages/TestResult";
import HeaderBar from "./components/HeaderBar";
import Box from "@mui/material/Box";
import Toolbar from "@mui/material/Toolbar";
import LoginPage from "./components/pages/LoginPage";
import { useUserContext } from "./components/UserContext";
import CircularProgress from "@mui/material/CircularProgress";
import Typography from "@mui/material/Typography";

const FullLayout = () => (
  <Box className="flex">
    <HeaderBar />
    <Sidebar />
    <Box component="main" className="flex-grow p-5">
      <Toolbar />
      <Outlet />
    </Box>
  </Box>
);

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

  return (
    <>
      {loadingUser ? (
        <LoadingPage />
      ) : (
        <Routes>
          <Route path="/login" element={<LoginPage />} />
          <Route element={user !== null ? <FullLayout /> : <Navigate to="/login" />}>
            <Route path="/" element={<Announcements />} />
            <Route path="/cases" element={<CaseList />} />
            <Route path="/cases/:caseid" element={<CasePage />} />
            <Route path="/notifications" element={<NotificationsListPage />} />
            <Route path="/profiles/:userid" element={<Profile />} />
            <Route path="/test-results" element={<TestResultList />} />
            <Route path="/test-results/:testresultid" element={<TestResultPage />} />
            <Route path="*" element={<NotFound />} />
          </Route>
        </Routes>
      )}
    </>
  );
};

export default App;

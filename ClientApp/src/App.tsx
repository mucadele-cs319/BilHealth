import React from "react";
import { Outlet, Route, Routes } from "react-router-dom";
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

const App = () => {
  return (
    <>
      <Routes>
        <Route path="/login" element={<LoginPage />} />
        <Route element={<FullLayout />}>
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
    </>
  );
};

export default App;

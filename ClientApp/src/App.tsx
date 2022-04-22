import React from "react";
import "./App.css";
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

const SidebarLayout = () => (
  <>
    <Sidebar />
    <Outlet />
  </>
);

const App = (): JSX.Element => {
  return (
    <div id="app">
      <Routes>
        <Route path="/login" />
        <Route path="/logout" />
        <Route element={<SidebarLayout />}>
          <Route path="/" element={<Announcements />} />
          <Route path="/cases" element={<CaseList />} />
          <Route path="/cases/:caseid" element={<CasePage />} />
          <Route path="/notifications" element={<NotificationsListPage />} />
          <Route path="/profiles/:userid" element={<Profile />} />
          <Route path="/test-results" element={<TestResultList />} />
          <Route path="/test-results/:testresultid" element={<TestResultPage />} />
        </Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </div>
  );
};

export default App;

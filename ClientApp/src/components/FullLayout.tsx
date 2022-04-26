import React, { useState } from "react";
import HeaderBar from "./HeaderBar";
import Toolbar from "@mui/material/Toolbar";
import Box from "@mui/material/Box";
import Sidebar from "./Sidebar";
import { Outlet } from "react-router-dom";

export interface MobileDrawerState {
  mobileDrawerOpen: boolean;
  toggleMobileDrawer: () => void;
}

const FullLayout = ({ mobile }: { mobile: boolean }) => {
  const [mobileDrawerOpen, setMobileDrawerOpen] = useState(false);
  const toggleMobileDrawer = () => {
    setMobileDrawerOpen(!mobileDrawerOpen);
  };

  return (
    <Box className="flex">
      <HeaderBar mobile={mobile} drawerState={{ mobileDrawerOpen, toggleMobileDrawer }} />
      <Sidebar mobile={mobile} drawerState={{ mobileDrawerOpen, toggleMobileDrawer }} />
      <Box component="main" className="flex-grow p-5">
        <Toolbar />
        <Outlet />
      </Box>
    </Box>
  );
};

export default FullLayout;

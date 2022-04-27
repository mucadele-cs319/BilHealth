import React from "react";
import Typography from "@mui/material/Typography";
import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import IconButton from "@mui/material/IconButton";
import { MobileDrawerState } from "./FullLayout";
import MenuIcon from "@mui/icons-material/Menu";

interface Props {
  mobile: boolean;
  drawerState: MobileDrawerState;
}

const HeaderBar = ({ mobile, drawerState }: Props) => {
  return (
    <AppBar position="fixed" sx={{ zIndex: (theme) => theme.zIndex.drawer + 1 }}>
      <Toolbar>
        {mobile ? (
          <IconButton color="inherit" edge="start" onClick={drawerState.toggleMobileDrawer} sx={{ mr: 2 }}>
            <MenuIcon />
          </IconButton>
        ) : null}
        <Typography variant="h6" noWrap component="div">
          BilHealth
        </Typography>
      </Toolbar>
    </AppBar>
  );
};

export default HeaderBar;

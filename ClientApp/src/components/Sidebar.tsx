import React, { useMemo, useState } from "react";
import { LinkProps, Link as RouterLink, useLocation, useNavigate } from "react-router-dom";
import Campaign from "@mui/icons-material/Campaign";
import Drawer from "@mui/material/Drawer";
import Toolbar from "@mui/material/Toolbar";
import Box from "@mui/material/Box";
import ListItemText from "@mui/material/ListItemText";
import ListItemIcon from "@mui/material/ListItemIcon";
import List from "@mui/material/List";
import ListItemButton from "@mui/material/ListItemButton";
import AccountBoxIcon from "@mui/icons-material/AccountBox";
import NotificationsIcon from "@mui/icons-material/Notifications";
import BiotechIcon from "@mui/icons-material/Biotech";
import MedicalServicesIcon from "@mui/icons-material/MedicalServices";
import LogoutIcon from "@mui/icons-material/Logout";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";
import { useUserContext } from "./UserContext";
import { MobileDrawerState } from "./FullLayout";
import CalculateIcon from "@mui/icons-material/Calculate";
import AdminPanelSettingsIcon from "@mui/icons-material/AdminPanelSettings";

interface ListItemLinkProps {
  nested?: boolean;
  icon?: React.ReactElement;
  primary: string;
  to: string;
}

const ListItemLink = ({ nested = false, icon, primary, to }: ListItemLinkProps) => {
  const location = useLocation();

  const renderLink = useMemo(
    () =>
      React.forwardRef<HTMLAnchorElement, Omit<LinkProps, "to">>(function Link(itemProps, ref) {
        return <RouterLink to={to} ref={ref} {...itemProps} role={undefined} />;
      }),
    [to],
  );

  return (
    <li>
      <ListItemButton sx={nested ? { pl: 4 } : null} selected={to === location.pathname} component={renderLink}>
        {icon ? <ListItemIcon>{icon}</ListItemIcon> : null}
        <ListItemText primary={primary} />
      </ListItemButton>
    </li>
  );
};

interface Props {
  mobile: boolean;
  drawerState: MobileDrawerState;
}

const sidebarWidth = 240;
const Sidebar = ({ mobile, drawerState }: Props) => {
  const { user, logout } = useUserContext();

  const [logoutAttempt, setLogoutAttempt] = useState(false);
  const navigate = useNavigate();

  const handleLogoutAttempt = () => {
    setLogoutAttempt(true);
  };

  const handleLogoutActual = async () => {
    await logout();
    navigate("/login");
  };

  const handleLogoutCancel = () => {
    setLogoutAttempt(false);
  };

  const drawerContents = (
    <>
      <Toolbar />
      <Box className="overflow-auto">
        <List>
          <ListItemLink primary="Profile" to={`/profiles/${user?.id}`} icon={<AccountBoxIcon />} />
          <ListItemLink nested primary="Notifications" to="/notifications" icon={<NotificationsIcon />} />
          <ListItemLink nested primary="Test Results" to="/test-results" icon={<BiotechIcon />} />
          <ListItemLink primary="Announcements" to="/" icon={<Campaign />} />
          <ListItemLink primary="Cases" to="/cases" icon={<MedicalServicesIcon />} />
          <ListItemLink primary="Calculators" to="/calculators" icon={<CalculateIcon />} />
        </List>
      </Box>
      <Box sx={{ marginTop: "auto", flexGrow: 0 }}>
        {["Admin", "Patient"].some((type) => user?.userType == type) ? (
          <ListItemLink primary="Staff Panel" to="/staffpanel" icon={<AdminPanelSettingsIcon />} />
        ) : null}
        <ListItemButton onClick={handleLogoutAttempt}>
          <ListItemIcon>
            <LogoutIcon />
          </ListItemIcon>
          <ListItemText primary="Log Out" />
        </ListItemButton>
      </Box>
      <Dialog open={logoutAttempt} onClose={handleLogoutCancel}>
        <DialogTitle>Are you sure you want to log out?</DialogTitle>
        <DialogContent>
          <DialogContentText>If you confirm, you will be redirected to the login page.</DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleLogoutCancel}>No</Button>
          <Button onClick={handleLogoutActual} autoFocus>
            Yes
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );

  return (
    <>
      {mobile ? (
        <Drawer
          variant="temporary"
          container={window.document.body}
          open={drawerState.mobileDrawerOpen}
          onClose={drawerState.toggleMobileDrawer}
          ModalProps={{
            keepMounted: true,
          }}
          className="flex-shrink-0 flex flex-col"
          sx={{
            display: { xs: "block", md: "none" },
            width: sidebarWidth,
            ["& .MuiDrawer-paper"]: { width: sidebarWidth, boxSizing: "border-box" },
          }}
        >
          {drawerContents}
        </Drawer>
      ) : (
        <Drawer
          variant="permanent"
          className="flex-shrink-0 flex flex-col"
          sx={{
            display: { xs: "none", md: "block" },
            width: sidebarWidth,
            ["& .MuiDrawer-paper"]: { width: sidebarWidth, boxSizing: "border-box" },
          }}
        >
          {drawerContents}
        </Drawer>
      )}
    </>
  );
};

export default Sidebar;

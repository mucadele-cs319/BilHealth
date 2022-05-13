import Autocomplete from "@mui/material/Autocomplete";
import CircularProgress from "@mui/material/CircularProgress";
import TextField from "@mui/material/TextField";
import React, { useEffect, useState } from "react";
import APIClient from "../util/API/APIClient";
import { SimpleUser, UserType } from "../util/API/APITypes";
import { fullNameify } from "../util/StringUtil";

interface Props {
  userType?: UserType | "all";
  label: string;
  value: SimpleUser | null;
  onChange: (e: React.SyntheticEvent<Element | Event>, v: SimpleUser | null) => void;
  disabled?: boolean;
}

const UserAutoComplete = ({ userType = "all", label, value, onChange, disabled = false }: Props) => {
  const [open, setOpen] = useState(false);
  const [options, setOptions] = useState<readonly SimpleUser[]>([]);
  const loading = open && options.length === 0;

  useEffect(() => {
    let active = true;

    if (!loading) {
      return undefined;
    }

    (async () => {
      const users = await APIClient.profiles.all(userType);

      if (active) {
        setOptions([...users]);
      }
    })();

    return () => {
      active = false;
    };
  }, [loading]);

  useEffect(() => {
    if (open === false) setOptions([]);
  }, [open]);

  return (
    <Autocomplete
      disabled={disabled}
      id="user-autocomplete-search"
      // sx={{ width: 300 }}
      open={open}
      onOpen={() => {
        setOpen(true);
      }}
      onClose={() => {
        setOpen(false);
      }}
      value={value}
      onChange={onChange}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      getOptionLabel={(option) => fullNameify(option)}
      options={options}
      loading={loading}
      renderInput={(params) => (
        <TextField
          {...params}
          label={label}
          InputProps={{
            ...params.InputProps,
            endAdornment: (
              <>
                {loading ? <CircularProgress color="inherit" size={20} /> : null}
                {params.InputProps.endAdornment}
              </>
            ),
          }}
        />
      )}
    />
  );
};

export default UserAutoComplete;

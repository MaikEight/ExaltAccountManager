import { Checkbox } from "@mui/material";

function DailyLoginCheckbox({ params, onChange, sx }) {
    
  const handleCheckboxChange = (event) => {
    event.stopPropagation();
    onChange(event);
  };

  return (
    <Checkbox
      sx={{ margin: 'auto', ...sx }}
      checked={params.value}
      onChange={handleCheckboxChange}
    />
  );
}

export default DailyLoginCheckbox;

import { Checkbox } from "@mui/material";

function DailyLoginCheckbox({ params, onChange }) {
    
  const handleCheckboxChange = (event) => {
    event.stopPropagation();
    onChange(event);
  };

  return (
    <Checkbox
      sx={{ margin: 'auto' }}
      checked={params.value}
      onChange={handleCheckboxChange}
    />
  );
}

export default DailyLoginCheckbox;

import { Checkbox } from "@mui/material";

function DailyLoginCheckbox({ params, onChange }) {
    return (
        <Checkbox
            sx={{ margin: 'auto' }}
            checked={params.value}
            onChange={onChange}
        />
    )
}

export default DailyLoginCheckbox;
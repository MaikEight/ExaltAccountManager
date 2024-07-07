import { Box } from "@mui/material";
import ComponentBox from "../ComponentBox";
import Character from "../Realm/Character";

function AccountView({ account }) {
    
    return (
        <ComponentBox
            title={account.name ? account.name : account.email}
            isCollapseable={true}
            innerSx={{
                dispaly: 'flex',
                flexDirection: 'coulmn',
                gap: 1
            }}
        >
            {/* Characters */}
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    gap: 1,
                    flexWrap: 'wrap',
                }}
            >
                {
                    account.character &&
                    account.character.map((char, index) => {
                        return (
                            <Character key={index} character={char} />
                        );
                    })
                }
            </Box>
        </ComponentBox>
    );
}

export default AccountView;
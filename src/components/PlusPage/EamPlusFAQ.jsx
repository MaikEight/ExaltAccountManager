import { Accordion, AccordionDetails, AccordionSummary, accordionSummaryClasses, Box, Typography } from "@mui/material";
import KeyboardArrowLeftIcon from '@mui/icons-material/KeyboardArrowLeft';

function EamPlusFAQ() {

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                mx: 2,
                mb: 2,
            }}
        >
            <Typography
                variant="h6"
                component={'h3'}
                sx={{
                    mb: 1,
                }}
            >
                Frequently Asked Questions
            </Typography>
            <FaqAccordion
                title="What is EAM Plus?"
                content={(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                        }}
                    >
                        <Typography>
                            EAM Plus is a premium subscription service that provides some additional features and benefits to enhance your experience with the Exalt Account Manager.
                        </Typography>
                        <Typography>
                            It is designed to support the development and maintenance of the EAM application, ensuring that it remains (mostly) free and open-source for everyone.
                        </Typography>
                    </Box>
                )}
            />
            <FaqAccordion
                title="Why did EAM get a paid subscription?"
                content={(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                        }}
                    >
                        <Typography>
                            EAM has been a free and open-source project since its first got released in 2020. However, maintaining and developing the application requires significant time and resources.
                        </Typography>
                        <Typography>
                            By introducing a paid subscription model, we aim to generate revenue that can be reinvested into the project, ensuring its sustainability and continued improvement.
                        </Typography>
                        <Typography>
                            The goal is not to make a profit, but to cover the costs associated with development, hosting, and other expenses.
                        </Typography>
                    </Box>
                )}
            />
            <FaqAccordion
                title="Is EAM becoming a paid application?"
                content={(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                        }}
                    >
                        <Typography>
                            No, EAM will remain a free and open-source application. The introduction of EAM Plus is meant to provide additional features for those who wish to support the project financially.
                        </Typography>
                        <Typography>
                            <b>All core functionalities of EAM will always remain free, forever.</b> Plus features are optional enhancements. No new features will be completely locked behind a paywall.
                        </Typography>
                    </Box>
                )}
            />
            <FaqAccordion
                title="EAM is never going to sell your data, right?"
                content={(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                        }}
                    >
                        <Typography>
                            Absolutely not! EAM is committed to user privacy and data security. We will never sell or share your personal data with third parties.
                        </Typography>
                    </Box>
                )}
            />
            <FaqAccordion
                title="What happens if no one subscribes to EAM Plus?"
                content={(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                        }}
                    >
                        <Typography>
                            Nothing. If no one subscribes to EAM Plus, the application will continue to function as it always has. The core features will remain available for free, and the project will continue to be maintained.
                        </Typography>
                        <Typography>
                            EAM is and will always be a hobby project, and the introduction of EAM Plus is simply an attempt to make it more sustainable in the long run.
                        </Typography>
                    </Box>
                )}
            />
            <FaqAccordion
                title="I think EAM Plus is too expensive"
                content={(
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            gap: 0.5,
                        }}
                    >
                        <Typography>
                            We understand that not everyone can afford a subscription. EAM Plus is priced to cover the costs of development and maintenance, but we also want to ensure that the application remains accessible to everyone.
                        </Typography>
                        <Typography>
                            If you are unable to afford EAM Plus, please reach out to us. We may be able to find a solution that works for you (e.g. a free trial or a discount).
                        </Typography>
                    </Box>
                )}
            />

        </Box>
    );
}

export default EamPlusFAQ;

function FaqAccordion({ title, content }) {
    return (
        <Accordion
            elevation={1}
            square={false}
            sx={{
                borderRadius: theme => `${theme.shape.borderRadius}px`,
                border: '1px solid',
                borderColor: 'divider',
                overflow: 'hidden',
                my: 0.25,
            }}
        >
            <AccordionSummary
                expandIcon={<KeyboardArrowLeftIcon />}
                aria-controls="panel1-content"
                id="panel1-header"
                sx={{
                    borderRadius: theme => `${theme.shape.borderRadius}px`,
                    borderBottom: 'none',
                    borderColor: 'divider',
                    backgroundColor: theme => theme.palette.background.paper,
                    [`& .${accordionSummaryClasses.expandIconWrapper}.${accordionSummaryClasses.expanded}`]:
                    {
                        transform: 'rotate(90deg)',
                    },
                }}
            >
                <Typography component="span">{title}</Typography>
            </AccordionSummary>
            <AccordionDetails
                sx={{
                    backgroundColor: theme => theme.palette.background.paperLight,
                    borderRadius: theme => `${theme.shape.borderRadius}px`,
                }}
            >
                {content}
            </AccordionDetails>
        </Accordion>
    );
}
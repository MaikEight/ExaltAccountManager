import useWidgets from "../../../../hooks/useWidgets";
import { Box } from '@mui/material';
import { Widgets } from "../../Widgets";
import { useState } from 'react';
import {
    DndContext,
    closestCenter,
    KeyboardSensor,
    PointerSensor,
    useSensor,
    useSensors,
    DragOverlay,
} from '@dnd-kit/core';
import {
    arrayMove,
    SortableContext,
    sortableKeyboardCoordinates,
    verticalListSortingStrategy,
} from '@dnd-kit/sortable';

/**
 * This component renders all active widgets for the current WidgetBar Type in a grid layout.
 * The Grid has fixed dimensions per column and the amount of columns is determined by the slots in the WidgetBar settings.
 * Each widget can span multiple columns depending on its size settings.
 * Each widget can have its own height depending on its content.
 * Widgets can be rearranged via drag and drop.
 * @returns JSX Component
 */
function WidgetGrid() {
    const { widgetBarState, widgetBarConfig, reorderWidgets } = useWidgets();
    const [activeId, setActiveId] = useState(null);

    const slots = widgetBarConfig?.slots || 1;
    const editMode = widgetBarState?.editMode || false;
    const activeWidgets = widgetBarConfig?.activeWidgets || [];

    const sensors = useSensors(
        useSensor(PointerSensor, {
            activationConstraint: {
                distance: 8, // 8px movement required before drag starts
            },
        }),
        useSensor(KeyboardSensor, {
            coordinateGetter: sortableKeyboardCoordinates,
        })
    );

    const handleDragStart = (event) => {
        setActiveId(event.active.id);
    };

    const handleDragEnd = (event) => {
        if (!editMode) return;
        
        const { active, over } = event;

        if (active.id !== over?.id && over) {
            const oldIndex = activeWidgets.indexOf(active.id);
            const newIndex = activeWidgets.indexOf(over.id);

            const newOrder = arrayMove(activeWidgets, oldIndex, newIndex);
            reorderWidgets(newOrder);
        }

        setActiveId(null);
    };

    const handleDragCancel = () => {
        setActiveId(null);
    };

    return (
        <DndContext
            sensors={sensors}
            collisionDetection={closestCenter}
            onDragStart={handleDragStart}
            onDragEnd={handleDragEnd}
            onDragCancel={handleDragCancel}
        >
            <SortableContext
                items={activeWidgets}
                strategy={verticalListSortingStrategy}
                disabled={!editMode}
            >
                <Box
                    sx={{
                        display: 'grid',
                        gridTemplateColumns: `repeat(${slots}, 1fr)`,
                        gap: 1,
                        width: '100%',
                    }}
                >
                    {
                        activeWidgets.map((widgettype) => {
                            
                            const type = Widgets.getWidgetByType(widgettype);
                            
                            if (!type) return null;
                            const WidgetComponent = type.Component;
                            
                            return (
                                <WidgetComponent 
                                    key={widgettype}
                                    type={type}
                                    widgetId={widgettype}
                                />
                            );
                        })
                    }
                </Box>
            </SortableContext>
            <DragOverlay>
                {activeId ? (() => {
                    const type = Widgets.getWidgetByType(activeId);
                    if (!type) return null;
                    const WidgetComponent = type.Component;
                    
                    return (
                        <Box
                            sx={{
                                width: '100%',
                                opacity: 0.9,
                            }}
                        >
                            <WidgetComponent 
                                type={type}
                                widgetId={activeId}
                            />
                        </Box>
                    );
                })() : null}
            </DragOverlay>
        </DndContext>
    );
}

export default WidgetGrid;
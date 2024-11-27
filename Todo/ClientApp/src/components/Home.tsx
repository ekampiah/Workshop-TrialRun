import { ChangeEvent, useCallback, useEffect, useState } from "react";
import { Item } from "../model/Item";
import axios, { AxiosResponse } from "axios";
import { useForm } from "react-hook-form";
import {
  Button,
  Checkbox,
  CheckboxOnChangeData,
  createTableColumn,
  DataGrid,
  DataGridBody,
  DataGridCell,
  DataGridHeader,
  DataGridHeaderCell,
  DataGridRow,
  Drawer,
  TableCellLayout,
  TableColumnDefinition,
} from "@fluentui/react-components";

export const Home = () => {
  const [items, setItems] = useState<Item[]>([]);
  const [isPanelOpen, setIsPanelOpen] = useState<boolean>(false);
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm({
    defaultValues: { title: "", description: "" },
  });

  useEffect(() => {
    if (!!items.length) return;
    console.log("loading items");

    axios
      .get(process.env.REACT_APP_API_URL + "/todos")
      .then((response: AxiosResponse<Item[]>) => {
        setItems(response.data);
      })
      .catch(console.log);
  }, [items]);

  const updateItemCompleted = useCallback(
    (id: string, checked: boolean) => {
      var index = items.findIndex((i) => i.id === id);
      if (index < 0) return;

      axios
        .put(process.env.REACT_APP_API_URL + `/todos/${id}`, {
          ...items[index],
          isCompleted: checked,
        })
        .then((response: AxiosResponse<Item>) => {
          items[index] = response.data;
        })
        .catch(console.log);
    },
    [items]
  );

  const columns: TableColumnDefinition<Item>[] = [
    createTableColumn<Item>({
      columnId: "title",
      compare: (a, b) => {
        return a.title.localeCompare(b.title);
      },
      renderHeaderCell: () => {
        return "Title";
      },
      renderCell: (item) => {
        return <TableCellLayout>{item.title}</TableCellLayout>;
      },
    }),
    createTableColumn<Item>({
      columnId: "description",
      compare: (a, b) => {
        return a.title.localeCompare(b.title);
      },
      renderHeaderCell: () => {
        return "Description";
      },
      renderCell: (item) => {
        return <TableCellLayout>{item.description}</TableCellLayout>;
      },
    }),
    createTableColumn<Item>({
      columnId: "completed",
      compare: (a, b) => {
        return a.title.localeCompare(b.title);
      },
      renderHeaderCell: () => {
        return "Completed";
      },
      renderCell: (item) => {
        return (
          <Checkbox
            defaultChecked={item.isCompleted}
            onChange={(
              ev: ChangeEvent<HTMLInputElement>,
              data: CheckboxOnChangeData
            ) => updateItemCompleted(item.id, !!data.checked)}
          />
        );
      },
    }),
    createTableColumn<Item>({
      columnId: "delete",
      compare: (a, b) => {
        return a.title.localeCompare(b.title);
      },
      renderHeaderCell: () => {
        return "";
      },
      renderCell: (item) => {
        return <Button onClick={() => deleteItem(item.id)}>Delete</Button>;
      },
    }),
  ];

  const addNewItem = useCallback(
    (data: { title?: string; description?: string }) => {
      axios
        .post(process.env.REACT_APP_API_URL + "/todos", data)
        .then((response: AxiosResponse<Item>) => {
          setItems((prevItems) => [...prevItems, response.data]);
          setIsPanelOpen(false);
          reset();
        })
        .catch(console.log);
    },
    [setItems, setIsPanelOpen, reset]
  );

  const deleteItem = useCallback(
    (id: string) => {
      axios
        .delete(process.env.REACT_APP_API_URL + `/todos/${id}`)
        .then((response: AxiosResponse<boolean>) => {
          if (response.data) {
            setItems(items.filter((item) => item.id !== id));
            console.log("Successfully deleted");
          } else console.log("Delete unsucessful");
        })
        .catch(console.log);
    },
    [items, setItems]
  );

  return (
    <div>
      <h1>Todos</h1>
      <Button onClick={() => setIsPanelOpen(true)}>Add new item</Button>
      <DataGrid items={items} columns={columns}>
        <DataGridHeader>
          <DataGridRow
            selectionCell={{
              checkboxIndicator: { "aria-label": "Select all rows" },
            }}
          >
            {({ renderHeaderCell }) => (
              <DataGridHeaderCell>{renderHeaderCell()}</DataGridHeaderCell>
            )}
          </DataGridRow>
        </DataGridHeader>
        <DataGridBody<Item>>
          {({ item, rowId }) => (
            <DataGridRow<Item>
              key={rowId}
              selectionCell={{
                checkboxIndicator: { "aria-label": "Select row" },
              }}
            >
              {({ renderCell }) => (
                <DataGridCell>{renderCell(item)}</DataGridCell>
              )}
            </DataGridRow>
          )}
        </DataGridBody>
      </DataGrid>
      <Drawer
        open={isPanelOpen}
        onOpenChange={(_, { open }) => setIsPanelOpen(open)}
        type="overlay"
      >
        <h1>Add new item</h1>
        <form onSubmit={handleSubmit(addNewItem)}>
          <input
            {...register("title", { required: "Title is required" })}
            placeholder="Title"
          />
          <p>{errors.title?.message}</p>
          <textarea
            {...register("description")}
            rows={4}
            placeholder="Description"
          />
          <p>{errors.description?.message}</p>
          <input type="submit" />
        </form>
      </Drawer>
    </div>
  );
};

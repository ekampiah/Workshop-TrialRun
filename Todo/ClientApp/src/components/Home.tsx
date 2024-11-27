import { useCallback, useEffect, useState } from "react";
import {
  DetailsList,
  DetailsListLayoutMode,
  IColumn,
} from "@fluentui/react/lib/DetailsList";
import { Item } from "../model/Item";
import {
  Checkbox,
  DefaultButton,
  Panel,
  PanelType,
  PrimaryButton,
} from "@fluentui/react";
import axios, { AxiosResponse } from "axios";
import { useForm } from "react-hook-form";

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

  const columns: IColumn[] = [
    {
      key: "indexColumn",
      name: "Index",
      ariaLabel: "Column operations for File type, Press to sort on File type",
      isIconOnly: true,
      minWidth: 16,
      maxWidth: 16,
      onRender: (item: Item, index: number | undefined) => (
        <p>{!index || index === 0 ? 1 : index + 1}</p>
      ),
    },
    {
      key: "titleColumn",
      name: "Title",
      fieldName: "title",
      minWidth: 16,
      maxWidth: 150,
    },
    {
      key: "descriptionColumn",
      name: "Description",
      fieldName: "description",
      minWidth: 16,
      maxWidth: 300,
    },
    {
      key: "completedColumn",
      name: "Completed",
      fieldName: "isCompleted",
      minWidth: 16,
      maxWidth: 100,
      onRender: (item: Item) => (
        <Checkbox
          defaultChecked={item.isCompleted}
          onChange={(event, checked?: boolean) =>
            updateItemCompleted(item.id, !!checked)
          }
        />
      ),
    },
    {
      key: "deletedColumn",
      name: "Deleted",
      minWidth: 16,
      maxWidth: 16,
      onRender: (item: Item) => (
        <PrimaryButton onClick={() => deleteItem(item.id)} text="Delete" />
      ),
    },
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
    [items, isPanelOpen, setItems, setIsPanelOpen]
  );

  const deleteItem = useCallback((id: string) => {
    axios
      .delete(process.env.REACT_APP_API_URL + `/todos/${id}`)
      .then((response: AxiosResponse<boolean>) => {
        if (response.data) {
          setItems(items.filter((item) => item.id !== id));
          console.log("Successfully deleted");
        } else console.log("Delete unsucessful");
      })
      .catch(console.log);
  }, []);

  return (
    <div>
      <h1>Todos</h1>
      <DefaultButton text="Add new item" onClick={() => setIsPanelOpen(true)} />
      <DetailsList
        items={items}
        columns={columns}
        layoutMode={DetailsListLayoutMode.justified}
        isHeaderVisible={true}
        enterModalSelectionOnTouch={true}
        ariaLabelForSelectionColumn="Toggle selection"
        ariaLabelForSelectAllCheckbox="Toggle selection for all items"
        checkButtonAriaLabel="select row"
      />
      <Panel
        isOpen={isPanelOpen}
        onDismiss={() => setIsPanelOpen(false)}
        type={PanelType.medium}
        closeButtonAriaLabel="Close"
        headerText="Add new item"
      >
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
      </Panel>
    </div>
  );
};

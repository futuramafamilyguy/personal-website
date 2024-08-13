import { AxiosResponse } from "axios";
import React, { FormEvent, useEffect, useState } from "react";
import ReactDom from "react-dom";

import { useYear } from "../../contexts/YearContext";
import {
  debouncedCreatePicture,
  debouncedDeleteImage,
  debouncedDeletePicture,
  debouncedFetchCinemas,
  debouncedUpdatePicture,
  debouncedUploadImage,
  makeDebouncedRequest,
} from "../../personalWebsiteApi";
import Cinema from "../../types/Cinema";
import Picture from "../../types/Picture";
import styles from "./NewPictureModal.module.css";

interface NewPictureModalProps {
  isOpen: boolean;
  onClose: () => void;
  picture: Picture | null;
  setTrigger: () => void;
}

interface CreatePictureResponse {
  id: string;
}

interface UploadImageResponse {
  imageUrl: string;
}

const NewPictureModal: React.FC<NewPictureModalProps> = ({
  isOpen,
  onClose,
  picture,
  setTrigger,
}) => {
  const [name, setName] = useState("");
  const [yearReleased, setYearReleased] = useState("");
  const [image, setImage] = useState<File | null>(null);
  const [imageUrl, setImageUrl] = useState("");
  const [monthWatched, setMonthWatched] = useState("");
  const [cinemaId, setCinemaId] = useState("");
  const [zinger, setZinger] = useState("");
  const [alias, setAlias] = useState("");
  const [isFavorite, setIsFavorite] = useState(false);

  const [result, setResult] = useState("");

  const [cinemas, setCinemas] = useState<Cinema[]>([]);
  const year = useYear();

  useEffect(() => {
    setResult("");
    setName(picture ? picture.name : "");
    setYearReleased(picture ? picture.yearReleased.toString() : "");
    setImage(null);
    setImageUrl(picture && picture.imageUrl ? picture.imageUrl : "");
    setMonthWatched(picture ? picture.monthWatched.toString() : "");
    setCinemaId(picture ? picture.cinema.id : "");
    setZinger(picture && picture.zinger ? picture.zinger : "");
    setAlias(picture && picture.alias ? picture.alias : "");
    setIsFavorite(picture ? picture.isFavorite : false);
  }, [isOpen]);

  useEffect(() => {
    const fetchCinemas = () => {
      makeDebouncedRequest(debouncedFetchCinemas, {
        url: "/cinemas",
      })
        .then((response: AxiosResponse<Cinema[]>) => {
          setCinemas(response.data);
        })
        .catch((error: any) => {
          console.error("Error fetching cinemas:", error);
        });
    };

    fetchCinemas();
  }, []);

  const createPicture = () => {
    return makeDebouncedRequest(debouncedCreatePicture, {
      url: `/pictures/${year}`,
      method: "post",
      headers: {
        "Content-Type": "application/json",
      },
      data: JSON.stringify({
        name: name,
        monthWatched: monthWatched ? parseInt(monthWatched, 10) : null,
        yearReleased: yearReleased,
        cinemaId: cinemaId,
        zinger: zinger ? zinger : null,
        alias: alias ? alias : null,
      }),
    });
  };

  const uploadImage = (id: string) => {
    const formData = new FormData();
    formData.append("imageFile", image!);

    return makeDebouncedRequest(debouncedUploadImage, {
      url: `/pictures/${year}/${id}/image`,
      method: "post",
      headers: {
        "Content-Type": "multipart/form-data",
      },
      data: formData,
    });
  };

  const deleteImage = (id: string) => {
    return makeDebouncedRequest(debouncedDeleteImage, {
      url: `/pictures/${id}/image`,
      method: "delete",
    });
  };

  const deletePicture = (id: string) => {
    return makeDebouncedRequest(debouncedDeletePicture, {
      url: `/pictures/${id}`,
      method: "delete",
    });
  };

  const updatePicture = (id: string, url: string | null) => {
    return makeDebouncedRequest(debouncedUpdatePicture, {
      url: `/pictures/${id}`,
      method: "put",
      headers: {
        "Content-Type": "application/json",
      },
      data: JSON.stringify({
        name: name,
        monthWatched: monthWatched
          ? parseInt(monthWatched, 10)
          : new Date().getMonth(),
        yearReleased: yearReleased,
        cinemaId: cinemaId,
        zinger: zinger ? zinger : null,
        alias: alias ? alias : null,
        imageUrl: url ? url : !isDefaultImage(imageUrl) ? imageUrl : null,
        yearWatched: year,
        isFavorite: isFavorite,
      }),
    });
  };

  const orchestrateCreatePicture = () => {
    let id: string;
    createPicture()
      .then((response: AxiosResponse<CreatePictureResponse>) => {
        if (response.status === 200) {
          if (image) {
            id = response.data.id;
            return uploadImage(response.data.id);
          } else {
            return Promise.resolve({ data: { skip: true } } as AxiosResponse<{
              skip: boolean;
            }>);
          }
        } else {
          throw new Error(`Error creating picture ${response.status}`);
        }
      })
      .then(
        (
          response:
            | AxiosResponse<UploadImageResponse>
            | AxiosResponse<{ skip: boolean }>
        ) => {
          if ("skip" in response.data && response.data.skip) {
            return Promise.resolve({ data: { skip: true } } as AxiosResponse<{
              skip: boolean;
            }>);
          }
          if ("imageUrl" in response.data && response.status === 200) {
            return updatePicture(id, response.data.imageUrl);
          } else {
            throw new Error(`Error uploading image ${response.status}`);
          }
        }
      )
      .then((response: AxiosResponse | AxiosResponse<{ skip: boolean }>) => {
        if (
          ("skip" in response.data && response.data.skip) ||
          response.status === 200
        ) {
          setResult("Successfully created picture");
          setTrigger();
          onClose();
        } else {
          throw new Error(`Error updating picture ${response.status}`);
        }
      })
      .catch((error: any) => {
        console.error("Error creating picture:", error);
        setResult("Error creating picture");
      });
  };

  const orchestrateUpdatePicture = () => {
    if (image) {
      if (isDefaultImage(picture!.imageUrl)) {
        uploadImage(picture!.id)
          .then((response: AxiosResponse<UploadImageResponse>) => {
            if (response.status === 200) {
              return updatePicture(picture!.id, response.data.imageUrl);
            } else {
              throw new Error(`Error uploading image ${response.status}`);
            }
          })
          .then((response) => {
            if (response.status === 200) {
              setResult("Successfully updated picture");
              setTrigger();
              onClose();
            } else {
              throw new Error(`Error updating picture ${response.status}`);
            }
          })
          .catch((error: any) => {
            console.error("Error updating picture:", error);
            setResult("Error updating picture");
          });
      } else {
        deleteImage(picture!.id)
          .then((response) => {
            if (response.status === 204) {
              return uploadImage(picture!.id);
            } else {
              throw new Error(
                `Error deleting picture image ${response.status}`
              );
            }
          })
          .then((response: AxiosResponse<UploadImageResponse>) => {
            if (response.status === 200) {
              return updatePicture(picture!.id, response.data.imageUrl);
            } else {
              throw new Error(`Error uploading image ${response.status}`);
            }
          })
          .then((response) => {
            if (response.status === 200) {
              setResult("Successfully updated picture");
              setTrigger();
              onClose();
            } else {
              throw new Error(`Error updating picture ${response.status}`);
            }
          })
          .catch((error: any) => {
            console.error("Error updating picture:", error);
            setResult("Error updating picture");
          });
      }
    } else {
      updatePicture(picture!.id, null)
        .then((response) => {
          if (response.status === 200) {
            setResult("Successfully updated picture");
            setTrigger();
            onClose();
          } else {
            throw new Error(`Error updating picture ${response.status}`);
          }
        })
        .catch((error: any) => {
          console.error("Error updating picture:", error);
          setResult("Error updating picture");
        });
    }
  };

  const orchestrateDeletePicture = () => {
    if (!isDefaultImage(picture!.imageUrl)) {
      deleteImage(picture!.id)
        .then((response) => {
          if (response.status === 204) {
            return deletePicture(picture!.id);
          } else {
            throw new Error(`Error deleting picture image ${response.status}`);
          }
        })
        .then((response) => {
          if (response.status === 204) {
            setResult("Successfully deleted picture");
            setTrigger();
            onClose();
          } else {
            throw new Error(`Error deleting picture ${response.status}`);
          }
        })
        .catch((error: any) => {
          console.error("Error deleting picture:", error);
          setResult("Error deleting picture");
        });
    } else {
      deletePicture(picture!.id)
        .then((response) => {
          if (response.status === 204) {
            setResult("Successfully deleted picture");
            setTrigger();
            onClose();
          } else {
            throw new Error(`Error deleting picture ${response.status}`);
          }
        })
        .catch((error: any) => {
          console.error("Error deleting picture:", error);
          setResult("Error deleting picture");
        });
    }
  };

  const handleSubmit = (event: FormEvent) => {
    event.preventDefault();
    if (picture) {
      orchestrateUpdatePicture();
    } else {
      orchestrateCreatePicture();
    }
  };

  const handleDelete = (event: FormEvent) => {
    event.preventDefault();
    orchestrateDeletePicture();
  };

  const isDefaultImage = (url: string) => url.includes("/assets/404");

  if (!isOpen) return null;

  return ReactDom.createPortal(
    <>
      <div className={styles.overlay} onClick={onClose}></div>
      <div className={styles.modal}>
        <div className={styles.textContainer}>
          {picture ? (
            <h5 className={styles.title}>Update Picture</h5>
          ) : (
            <h5 className={styles.title}>Add New Picture for {year}</h5>
          )}
          <form onSubmit={handleSubmit}>
            <div className={styles.formGroup}>
              <label>Name</label>
              <input
                type="text"
                id="name"
                value={name}
                onChange={(e) => setName(e.target.value)}
                required
                className={styles.inputField}
              ></input>
            </div>
            <div className={styles.formGroup}>
              <label>Year Released</label>
              <input
                type="text"
                id="year_released"
                value={yearReleased}
                onChange={(e) => setYearReleased(e.target.value)}
                required
              ></input>
            </div>
            <div className={styles.formGroup}>
              <label>Image</label>
              <input
                type="file"
                onChange={(e) =>
                  setImage(e.target.files ? e.target.files[0] : null)
                }
              />
            </div>
            <div className={styles.formGroup}>
              <label>Month Watched</label>
              <input
                type="text"
                id="month_watched"
                value={monthWatched}
                onChange={(e) => setMonthWatched(e.target.value)}
              ></input>
            </div>
            <div className={styles.formGroup}>
              <label>Cinema</label>
              <input
                type="text"
                id="cinema_id"
                value={
                  cinemaId !== ""
                    ? cinemas.filter((cinema) => cinema.id === cinemaId)[0].name
                    : ""
                }
                onChange={(e) => setCinemaId(e.target.value)}
                className={styles.hiddenInput}
              ></input>
              <select
                id="cinema_id"
                value={cinemaId}
                onChange={(e) => setCinemaId(e.target.value)}
                required
              >
                <option value="">
                  {cinemaId !== ""
                    ? cinemas.filter((cinema) => cinema.id === cinemaId)[0].name
                    : "Select a cinema"}
                </option>
                {cinemas.map((cinema) => (
                  <option key={cinema.id} value={cinema.id}>
                    {cinema.name}
                  </option>
                ))}
              </select>
            </div>
            <div className={styles.formGroup}>
              <label>Favorite</label>
              <div>
                <input
                  type="checkbox"
                  id="favorite"
                  checked={isFavorite}
                  onChange={() => setIsFavorite(!isFavorite)}
                />
              </div>
            </div>
            <div className={styles.formGroup}>
              <label>Zinger</label>
              <input
                type="text"
                id="zinger"
                value={zinger}
                onChange={(e) => setZinger(e.target.value)}
              ></input>
            </div>
            <div className={styles.formGroup}>
              <label>Alias</label>
              <input
                type="text"
                id="alias"
                value={alias}
                onChange={(e) => setAlias(e.target.value)}
              ></input>
            </div>
            <div className={styles.buttonContainer}>
              <div>
                <button className={styles.button} type="submit">
                  Submit
                </button>
                <span className={styles.result} id="result">
                  {result}
                </span>
              </div>
              {picture ? (
                <div>
                  <button
                    className={styles.deleteButton}
                    onClick={handleDelete}
                  >
                    Delete
                  </button>
                </div>
              ) : null}
            </div>
          </form>
        </div>
      </div>
    </>,
    document.getElementById("portal")!
  );
};

export default NewPictureModal;

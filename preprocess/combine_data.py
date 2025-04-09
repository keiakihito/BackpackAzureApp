#!/usr/bin/env python
# coding: utf-8

# In[1]:


import pandas as pd


# In[12]:


# Read data
train = pd.read_csv('data/train.csv')
extra = pd.read_csv('data/training_extra.csv')


# In[13]:


# Consolidate with id
df = pd.merge(train, extra, on='id', how='left')


# In[14]:


print("📋 df.columns:")
for col in df.columns:
    print(f" - {col}")


# In[15]:


# Define Combined columns
base_columns = {
    "brand": ["Brand_x", "Brand_y"],
    "price": ["Price_x", "Price_y"],
    "weight_capacity_kg": ["Weight Capacity (kg)_x", "Weight Capacity (kg)_y"],
    "waterproof": ["Waterproof_x", "Waterproof_y"],
    "size": ["Size_x", "Size_y"],
    "color": ["Color_x", "Color_y"],
    "style": ["Style_x", "Style_y"],
    "compartments": ["Compartments_x", "Compartments_y"]
}


# In[17]:


# Integrate columns
for new_col, (x_col, y_col) in base_columns.items():
    df[new_col] = df[x_col].combine_first(df[y_col])


# In[18]:


# Integrate other columns
optional_columns = []
if "release_date" in df.columns:
    df["release_date"] = pd.to_datetime(df["release_date"], errors="coerce")
    optional_columns.append("release_date")
if "category" in df.columns:
    optional_columns.append("category")


# In[19]:


# waterproof Yes/No => 1/0
df["waterproof"] = df["waterproof"].map({"Yes": 1, "No": 0})


# In[20]:


# Desired columns to extract from df
final_columns = ["id"] + list(base_columns.keys()) + optional_columns


# In[21]:


# Check the column exist to prevent typo
print('Exist column in df.columns to check my intended column')
for col in final_columns:
    if col in df.columns:
        print(f" - {col} √")
    else:
        print(f" - {col} X (NOT FOUND)")


# In[22]:


# Extract data from specified column
df_clean = df[final_columns].dropna()


# In[23]:


df_clean.head(5)


# In[24]:


df_clean.to_csv('data/processed_data.csv', index=False)
print(f"✅processed_data.csv created!!✅")


# In[ ]:




